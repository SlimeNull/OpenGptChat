using System.Text.Json.Serialization;

namespace OpenAI.Chat
{
    public sealed class Delta
    {
        [JsonConstructor]
        public Delta(Role role, string content)
        {
            Role = role;
            Content = content;
        }

        [JsonInclude]
        [JsonPropertyName("role")]
        public Role Role { get; private set; }

        [JsonInclude]
        [JsonPropertyName("content")]
        public string Content { get; private set; }
    }
}