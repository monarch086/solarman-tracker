using Amazon.Lambda.Core;
using SolarmanTracker.Core;
using SolarmanTracker.Core.Repositories;
using SolarmanTracker.Core.SsmConfig;
using System.Text.Json.Nodes;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace TrackingLambda;

public class Function
{
    /// <summary>
    /// A function that reads data from Solarman devices and posts to Telegram.
    /// </summary>
    /// <param name="input">The event for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public async Task FunctionHandler(JsonObject input, ILambdaContext context)
    {
        var stage = Environment.GetEnvironmentVariable("STAGE");
        var configsRepository = new ConfigRepository(stage, context.Logger);
        var config = await SsmConfigBuilder.Build(stage, context.Logger);

        var devices = await configsRepository.GetConfigs();

        foreach (var device in devices)
        {
            if (device != null && device.IsActive)
            {
                var bot = new ChatBot(config.Token);
                var message = "Hello from SolarmanTracker";
                await bot.Post(message, device.ChatId);
            }
        }

        context.Logger.LogInformation("Devices processing complete.");
    }
}
