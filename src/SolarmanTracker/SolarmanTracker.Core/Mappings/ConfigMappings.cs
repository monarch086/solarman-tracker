using Amazon.DynamoDBv2.DocumentModel;
using SolarmanTracker.Core.DataModel;

namespace SolarmanTracker.Core.Mappings
{
    internal static class ConfigMappings
    {
        public static Config ToDeviceConfig(this Document document)
        {
            return new Config
            {
                AppId = document["AppId"].AsString(),
                AppSecret = document["AppSecret"].AsString(),
                DeviceSn = document["DeviceSn"].AsString(),
                IsActive = document["IsActive"].AsBoolean(),
                ChatId = document["ChatId"].AsString(),
            };
        }
    }
}
