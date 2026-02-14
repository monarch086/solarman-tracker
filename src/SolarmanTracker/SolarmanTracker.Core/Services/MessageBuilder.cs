using SolarmanTracker.Core.DataModel.DataLoaderModels;
using System.Text;

namespace SolarmanTracker.Core.Services
{
    public static class MessageBuilder
    {
        private const string CONFIG_ICON = "🎛";
        private const string ENERGY_ICON = "⚡";
        private const string CHECK_ICON = "✅";
        private const string CROSS_ICON = "❌";

        public static string Build(RealTimeStationResponse response)
        {
            var sb = new StringBuilder($"{ENERGY_ICON} <b>Статус інвертора:</b>\n");

            if (response.wirePower > 0)
            {
                sb.Append($"{CHECK_ICON} Електроенергія в мережі наявна.\n");
            }
            else
            {
                sb.Append($"{CROSS_ICON} Електроенергія в мережі відсутня.\n");
            }

            sb.Append($"Рівень заряду батарей: {response.batterySoc}%");
            sb.Append($"Час оновлення даних: {response.lastUpdateTime.ToString("O")}.");

            return sb.ToString();
        }
    }
}
