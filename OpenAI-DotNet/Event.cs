using System;
using System.Text.Json.Serialization;

namespace OpenAI
{
    public sealed class Event
    {
        [JsonInclude]
        [JsonPropertyName("object")]
        public string Object { get; private set; } = string.Empty;

        [JsonInclude]
        [JsonPropertyName("created_at")]
        public int CreatedAtUnixTime { get; private set; }

        [JsonIgnore]
        public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnixTime).DateTime;

        [JsonInclude]
        [JsonPropertyName("level")]
        public string Level { get; private set; } = string.Empty;

        [JsonInclude]
        [JsonPropertyName("message")]
        public string Message { get; private set; } = string.Empty;
    }
}
