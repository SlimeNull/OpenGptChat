using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenAI.Chat
{
    public sealed class ChatResponse : BaseResponse
    {
        [JsonConstructor]
        public ChatResponse(string id, string @object, int created, string model, Usage usage, IReadOnlyList<Choice> choices)
        {
            Id = id;
            Object = @object;
            Created = created;
            Model = model;
            Usage = usage;
            Choices = choices;
        }

        [JsonInclude]
        [JsonPropertyName("id")]
        public string Id { get; private set; }

        [JsonInclude]
        [JsonPropertyName("object")]
        public string Object { get; private set; }

        [JsonInclude]
        [JsonPropertyName("created")]
        public int Created { get; private set; }

        [JsonInclude]
        [JsonPropertyName("model")]
        public string Model { get; private set; }

        [JsonInclude]
        [JsonPropertyName("usage")]
        public Usage Usage { get; private set; }

        [JsonInclude]
        [JsonPropertyName("choices")]
        public IReadOnlyList<Choice> Choices { get; private set; }

        [JsonInclude]
        [JsonPropertyName("error")]
        public ChatError? Error { get; set; }


        [JsonIgnore]
        public Choice? FirstChoice => Choices.FirstOrDefault();
    }
}
