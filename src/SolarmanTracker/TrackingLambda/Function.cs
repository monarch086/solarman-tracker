using Amazon.Lambda.Core;
using SolarmanTracker.Core;
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
        var config = await SsmConfigBuilder.Build(stage, context.Logger);

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
                        continue;
                    }


                    var bot = new ChatBot(config.Token);
                    var message = MessageBuilder.Build(dataParsed);
                    await bot.Post(message, device.ChatId);

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
            }
        }

        context.Logger.LogInformation("Devices processing complete.");
    }
}
