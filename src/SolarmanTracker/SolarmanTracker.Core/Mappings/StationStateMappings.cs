using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using SolarmanTracker.Core.DataModel;

namespace SolarmanTracker.Core.Mappings
{
    internal static class StationStateMappings
    {
        public static Document ToDocument(this StationState record)
        {
            var attributes = new Dictionary<string, AttributeValue>()
              {
                  { "StationId", new AttributeValue { S = record.StationId }},
                  { "Date", new AttributeValue { S = record.Date }},
                  { "StateDate", new AttributeValue { S = record.StateDate }},
                  { "JsonData", new AttributeValue { S = record.JsonData }},
              };

            return Document.FromAttributeMap(attributes);
        }

        public static StationState ToStationState(this Document document)
        {
            return new StationState()
            {
                StationId = document["StationId"].AsString(),
                Date = document["Date"].AsString(),
                StateDate = document["StateDate"].AsString(),
                JsonData = document["JsonData"].AsString()
            };
        }
    }
}
