using SolarmanTracker.Core.Converters;
using SolarmanTracker.Core.DataModel.DataLoaderModels;
using System.Text.Json;

namespace SolarmanTracker.Core.DataModel
{
    public sealed class StationState
    {
        public string StationId { get; set; }

        public string Date { get; set; }

        public string StateDate { get; set; }

        public string JsonData { get; set; }

        public bool IsLostConnectionSent { get; set; }

        public StationState() { }

        public StationState(RealTimeStationResponse response, string stationId)
        {
            StationId = stationId;
            Date = DateTime.UtcNow.ToString("O");
            StateDate = response.lastUpdateTime.ToString("O");
            JsonData = JsonSerializer.Serialize(response, SerializationConstants.SerializerOptions);
            IsLostConnectionSent = false;
        }

        public RealTimeStationResponse ToResponse()
        {
            var response = JsonSerializer.Deserialize<RealTimeStationResponse>(JsonData, SerializationConstants.SerializerOptions)
                ?? new RealTimeStationResponse();

            return response;
        }
    }
}
