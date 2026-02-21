using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.Core;
using SolarmanTracker.Core.DataModel;
using SolarmanTracker.Core.Mappings;

namespace SolarmanTracker.Core.Repositories
{
    public sealed class StationStateRepository
    {
        private readonly AmazonDynamoDBClient client;
        private readonly Table stateTable;
        private readonly ILambdaLogger logger;

        public StationStateRepository(string stage, ILambdaLogger logger)
        {
            client = new AmazonDynamoDBClient();
            stateTable = new TableBuilder(client, $"SolarmanTracker.{stage}.StationStates")
                .AddHashKey("StationId", DynamoDBEntryType.String)
                .Build();
            this.logger = logger;
        }

        public async Task AddOrUpdate(StationState state)
        {
            await stateTable.PutItemAsync(state.ToDocument());
        }

        public async Task<StationState?> GetLatest(string stationId)
        {
            var keyExpression = new Expression();
            keyExpression.ExpressionStatement = "StationId = :v_stationId";
            keyExpression.ExpressionAttributeValues[":v_stationId"] = stationId;

            var config = new QueryOperationConfig()
            {
                Limit = 1,
                Select = SelectValues.AllAttributes,
                BackwardSearch = true,
                ConsistentRead = true,
                KeyExpression = keyExpression
            };

            var queryResult = stateTable.Query(config);
            logger.LogInformation($"[GetLatest] Query result count: {queryResult.Count}");

            var documents = await queryResult.GetNextSetAsync();

            if (documents.Count > 0)
            {
                return documents[0].ToStationState();
            }

            return null;
        }
    }
}
