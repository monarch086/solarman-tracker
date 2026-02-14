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
                StationId = document["StationId"].AsString(),
                AppId = document["AppId"].AsString(),
                AppSecret = document["AppSecret"].AsString(),
                IsActive = document["IsActive"].AsBoolean(),
                AccessToken = document["AccessToken"].AsString(),
                //AccessTokenExpDate = document["AccessTokenExpDate"].AsDateTimeUtc(),
                RefreshToken = document["RefreshToken"].AsString(),
                DeviceSn = document["DeviceSn"].AsString(),
                ChatId = document["ChatId"].AsString(),
            };
        }
    }
}
