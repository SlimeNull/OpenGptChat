
using System.Text.Json.Serialization;
using System.Text.Json;

namespace OpenGptChat.Utilities
{
    internal static class JsonHelper
    {
        public static JsonSerializerOptions ConfigurationOptions { get; } =
            new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,

                Converters =
                {
                    new JsonStringEnumConverter(),
                }
            };
    }
}