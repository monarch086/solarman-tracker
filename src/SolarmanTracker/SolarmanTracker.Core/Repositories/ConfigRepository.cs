using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.Core;
using SolarmanTracker.Core.DataModel;

namespace SolarmanTracker.Core.Repositories
{
    public sealed class ConfigRepository
    {
        private readonly AmazonDynamoDBClient client;
        private readonly Table configTable;
        private readonly ILambdaLogger logger;

        public ConfigRepository(string stage, ILambdaLogger logger)
        {
            client = new AmazonDynamoDBClient();
            configTable = new TableBuilder(client, $"SolarmanTracker.{stage}.Configs").Build();
            this.logger = logger;
        }

        public async Task<List<Config>> GetConfigs()
        {
            var configs = new List<Config>();

            var scanFilter = new ScanFilter();
            var scanResult = configTable.Scan(scanFilter);

            do
            {
                var documents = await scanResult.GetNextSetAsync();
                foreach (var document in documents)
                {
                    try
                    {
                        configs.Add(document.ToDeviceConfig());
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex.Message);
                    }
                }
            } while (!scanResult.IsDone);

            return configs;
        }
    }
}
