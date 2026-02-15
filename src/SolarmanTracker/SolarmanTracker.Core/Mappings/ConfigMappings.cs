using Amazon.DynamoDBv2.DocumentModel;
using SolarmanTracker.Core.DataModel;
using System.Globalization;

namespace SolarmanTracker.Core.Mappings
{
    internal static class ConfigMappings
    {
        private static DateTimeStyles styles = DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal;

        public static Config ToDeviceConfig(this Document document)
        {
            DateTime? accessTokenExpDate = DateTime.TryParse(document["AccessTokenExpDate"].AsString(), null, styles, out var dt)
                ? dt
                : null;

            return new Config
            {
                StationId = document["StationId"].AsString(),
                AppId = document["AppId"].AsString(),
                AppSecret = document["AppSecret"].AsString(),
                IsActive = document["IsActive"].AsBoolean(),
                AccessToken = document["AccessToken"].AsString(),
                AccessTokenExpDate = accessTokenExpDate,
                RefreshToken = document["RefreshToken"].AsString(),
                DeviceSn = document["DeviceSn"].AsString(),
                ChatId = document["ChatId"].AsString(),
            };
        }
    }
}
