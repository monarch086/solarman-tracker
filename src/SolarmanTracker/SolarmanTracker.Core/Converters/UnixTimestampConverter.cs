using System.Text.Json;
using System.Text.Json.Serialization;

namespace SolarmanTracker.Core.Converters
{
    public sealed class UnixTimestampConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                var unixTime = reader.GetInt64();
                return DateTimeOffset.FromUnixTimeSeconds(unixTime).UtcDateTime;
            }

            // Fallback: handle standard ISO 8601 strings if needed
            if (reader.TokenType == JsonTokenType.String)
            {
                return DateTime.Parse(reader.GetString()!);
            }

            throw new JsonException($"Unexpected token type {reader.TokenType} for DateTime.");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(new DateTimeOffset(value).ToUnixTimeSeconds());
        }
    }
}
