using Amazon.Lambda.Core;
using SolarmanTracker.Core;
using SolarmanTracker.Core.DataModel;
using SolarmanTracker.Core.DataModel.DataLoaderModels;
using SolarmanTracker.Core.Repositories;
using SolarmanTracker.Core.Services;
using SolarmanTracker.Core.SsmConfig;
using System.Text.Json;
using System.Text.Json.Nodes;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace TrackingLambda;

public class Function
{
    /// <summary>
    /// A function that reads data from Solarman devices and posts to Telegram.
    /// </summary>
    public async Task FunctionHandler(JsonObject input, ILambdaContext context)
    {
        var stage = Environment.GetEnvironmentVariable("STAGE");
        var configsRepository = new ConfigRepository(stage, context.Logger);
        var stateRepository = new StationStateRepository(stage, context.Logger);

        var messengerConfig = await SsmConfigBuilder.Build(stage, context.Logger);
        var bot = new ChatBot(stage, messengerConfig.Token);

        var dataLoader = new SolarmanDataLoader(context.Logger);

        var devices = await configsRepository.GetConfigs();

        foreach (var device in devices)
        {
            try
            {
                if (device != null && device.IsActive && int.TryParse(device.StationId, out int stationId))
                {
                    var realTimeData = await dataLoader.LoadRealTimeStationData(stationId, device.AccessToken);
                    var dataParsed = JsonSerializer.Deserialize<RealTimeStationResponse>(realTimeData);
                    if (dataParsed == null)
                    {
                        context.Logger.LogWarning($"Failed to parse real-time data for StationId: {device.StationId}. Skipping.");
                        await bot.PostError($"Failed to parse real-time data for StationId: {device.StationId}. Skipping.");
                        continue;
                    }
                    var latestState = await stateRepository.GetLatest(device.StationId);
                    var latestResponse = !string.IsNullOrWhiteSpace(latestState?.JsonData)
                        ? JsonSerializer.Deserialize<RealTimeStationResponse>(latestState.JsonData)
                        : null;

                    var isNewResponse = latestResponse == null || dataParsed.lastUpdateTime > latestResponse.lastUpdateTime;
                    var isStateChanged = latestResponse == null
                        || dataParsed.batterySoc != latestResponse.batterySoc
                        || dataParsed.isElectricityPresent != latestResponse.isElectricityPresent;

                    context.Logger.LogInformation($"Device with StationId: {device.StationId}, isNewResponse: {isNewResponse}.");
                    context.Logger.LogInformation($"Device with StationId: {device.StationId}, isStateChanged: {isStateChanged}.");

                    if (isStateChanged)
                    {
                        
                        var message = MessageBuilder.Build(dataParsed);
                        await bot.Post(message, device.ChatId);
                    }

                    if (isNewResponse)
                    {
                        await stateRepository.Add(new StationState(dataParsed, device.StationId));
                    }

                    await CheckTokenExpiration(device, bot);

                    context.Logger.LogInformation($"Device with StationId: {device.StationId} was processed successfully.");
                }
                else
                {
                    context.Logger.LogWarning($"Skipping device with StationId: {device?.StationId} - Invalid or inactive.");
                }
            }
            catch (Exception ex)
            {
                context.Logger.LogError($"Error processing device with StationId: {device.StationId} - {ex.Message}");
                await bot.PostError($"Error processing device with StationId: {device.StationId} - {ex.Message}");
            }
        }

        context.Logger.LogInformation("Devices processing complete.");
    }

    public async Task CheckTokenExpiration(Config device, ChatBot bot)
    {
        if (device.AccessTokenExpDate.HasValue && device.AccessTokenExpDate.Value <= DateTime.UtcNow.AddDays(7))
        {
            await bot.PostWarning(MessageBuilder.BuildTokenExpirationMessage(device));
        }
    }
}
