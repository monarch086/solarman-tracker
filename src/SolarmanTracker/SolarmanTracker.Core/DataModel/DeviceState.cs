using System.Text.Json.Serialization;

namespace SolarmanTracker.Core.DataModel
{
    public sealed class DeviceState
    {
        [JsonIgnore]
        public string DeviceId { get; set; } = string.Empty;

        [JsonIgnore]
        public DateTime Date { get; set; }

        public float SoCLevel { get; set; }
    }
}
