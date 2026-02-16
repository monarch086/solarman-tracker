using SolarmanTracker.Core.Converters;
using System.Text.Json.Serialization;

namespace SolarmanTracker.Core.DataModel.DataLoaderModels
{
    public sealed class RealTimeStationResponse
    {
        public float batterySoc { get; set; }

        public float generationTotal { get; set; }

        public float? usePower { get; set; }

        public float? wirePower { get; set; }

        public float? chargePower { get; set; }

        public float? dischargePower { get; set; }

        [JsonConverter(typeof(UnixTimestampConverter))]
        public DateTime lastUpdateTime { get; set; }

        public float? generationPower { get; set; }

        public float? gridPower { get; set; }

        public float? purchasePower { get; set; }

        public float? batteryPower { get; set; }

        public bool success { get; set; }

        [JsonIgnore]
        public bool isElectricityPresent => wirePower.HasValue && wirePower.Value > 0;
    }
}
