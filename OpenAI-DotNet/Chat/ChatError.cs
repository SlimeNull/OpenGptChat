using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenAI.Chat
{
    public class ChatError
    {
        [JsonConstructor]
        public ChatError(string message, string type, string param, string code)
        {
            Message = message;
            Type = type;
            Param = param;
            Code = code;
        }

        [JsonInclude]
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonInclude]
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonInclude]
        [JsonPropertyName("param")]
        public string Param { get; set; }

        [JsonInclude]
        [JsonPropertyName("code")]
        public string Code { get; set; }
    }
}
