using System;
using System.Text;
using System.Text.Json;

namespace OpenGptChat.Utilities
{
    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        static SnakeCaseNamingPolicy()
        {
            laziedInstance = new Lazy<SnakeCaseNamingPolicy>();
        }

        private SnakeCaseNamingPolicy()
        {

        }

        private static readonly Lazy<SnakeCaseNamingPolicy> laziedInstance;
        public static SnakeCaseNamingPolicy SnakeCase => laziedInstance.Value;

        public override string ConvertName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }

            var builder = new StringBuilder();
            builder.Append(char.ToLowerInvariant(name[0]));

            for (var i = 1; i < name.Length; i++)
            {
                if (char.IsUpper(name[i]))
                {
                    builder.Append('_');
                    builder.Append(char.ToLowerInvariant(name[i]));
                }
                else
                {
                    builder.Append(name[i]);
                }
            }

            return builder.ToString();
        }
    }
}
