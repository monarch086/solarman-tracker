using SolarmanTracker.Core.DataModel;
using SolarmanTracker.Core.DataModel.DataLoaderModels;
using SolarmanTracker.Core.Extensions;
using System.Text;

namespace SolarmanTracker.Core.Services
{
    public static class MessageBuilder
    {
        private const string CONFIG_ICON = "🎛";
        private const string ENERGY_ICON = "⚡";
        private const string CHECK_ICON = "✅";
        private const string CROSS_ICON = "❌";

        private const string TIME_FORMAT = @"dd MMM H:mm";

        public static string Build(RealTimeStationResponse response)
        {
            var sb = new StringBuilder($"{ENERGY_ICON} <b>Статус інвертора:</b>\n");

            if (response.isElectricityPresent)
            {
                sb.Append($"{CHECK_ICON} Електроенергія в мережі наявна.\n");
            }
            else
            {
                sb.Append($"{CROSS_ICON} Електроенергія в мережі відсутня.\n");
            }

            sb.Append($"Рівень заряду батарей: {response.batterySoc}%\n");
            sb.Append($"Час оновлення даних: {response.lastUpdateTime.ToKyivTime().ToString(TIME_FORMAT)}.");

            return sb.ToString();
        }

        public static string BuildTokenExpirationMessage(Config device)
        {
            var expirationDate = device.AccessTokenExpDate.HasValue
                ? device.AccessTokenExpDate.Value.ToKyivTime().ToString(TIME_FORMAT)
                : string.Empty;
            return $"Access token for StationId: {device.StationId} is expiring soon on {expirationDate}.";
        }

        public static string BuildLostConnectionMessage(RealTimeStationResponse response)
        {
            var lastUpdateTime = response.lastUpdateTime.ToKyivTime().ToString(TIME_FORMAT);
            return $"⚠️ Втрата зв'язку зі станцією з {lastUpdateTime}.";
        }
    }
}
