using System.Text.Json;
using System.Text.Json.Serialization;

namespace SolarmanTracker.Core.DataModel
{
    public sealed class DeviceStateRecord
    {
        public string DeviceId { get; set; }

        public string Date { get; set; }

        public string JsonData { get; set; }

        public DeviceStateRecord(DeviceState state)
        {
            JsonSerializerOptions options = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            DeviceId = state.DeviceId;
            Date = state.Date.ToString("O");
            JsonData = JsonSerializer.Serialize(state, options);
        }

        public DeviceState ToDeviceState()
        {
            JsonSerializerOptions options = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var deviceState = JsonSerializer.Deserialize<DeviceState>(JsonData, options) ?? new DeviceState();
            deviceState.DeviceId = DeviceId;
            deviceState.Date = DateTime.Parse(Date);

            return deviceState;
        }
    }
}
