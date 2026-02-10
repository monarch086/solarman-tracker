using Amazon.Lambda.Core;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;

namespace SolarmanTracker.Core.SsmConfig
{
    public class SsmConfigBuilder
    {
        private static string APP_NAME = "SolarmanTracker";
        private static string TOKEN_PARAM_NAME = "TelegramBot.Token";

        public static async Task<TelegramConfig> Build(string stage, ILambdaLogger logger)
        {
            try
            {
                var config = new TelegramConfig();

                var client = new AmazonSimpleSystemsManagementClient();

                var request = new GetParameterRequest()
                {
                    Name = $"/{APP_NAME}/{stage}/{TOKEN_PARAM_NAME}"
                };
                var result = await client.GetParameterAsync(request);
                config.Token = result.Parameter.Value;

                return config;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());

                return new TelegramConfig();
            }
        }
    }
}
