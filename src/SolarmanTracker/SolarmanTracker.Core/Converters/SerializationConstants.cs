using System.Text.Json;
using System.Text.Json.Serialization;

namespace SolarmanTracker.Core.Converters
{
    public static class SerializationConstants
    {
        public static JsonSerializerOptions SerializerOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }
}
