using OpenAI.Chat;
using OpenAI.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenAI
{
    /// <summary>
    /// Entry point to the OpenAI API, handling auth and allowing access to the various API endpoints
    /// </summary>
    public sealed class OpenAIClient
    {
        /// <summary>
        /// Creates a new entry point to the OpenAPI API, handling auth and allowing access to the various API endpoints
        /// </summary>
        /// <param name="openAIAuthentication">
        /// The API authentication information to use for API calls,
        /// or <see langword="null"/> to attempt to use the <see cref="OpenAI.OpenAIAuthentication.Default"/>,
        /// potentially loading from environment vars or from a config file.
        /// </param>
        /// <param name="clientSettings">
        /// Optional, <see cref="OpenAIClientSettings"/> for specifying OpenAI deployments to Azure or proxy domain.
        /// </param>
        /// <param name="client">A <see cref="HttpClient"/>.</param>
        /// <exception cref="AuthenticationException">Raised when authentication details are missing or invalid.</exception>
        public OpenAIClient(OpenAIAuthentication openAIAuthentication, OpenAIClientSettings clientSettings, HttpClient client)
            : this(openAIAuthentication, clientSettings)
        {
            Client = SetupClient(client);
        }

        /// <summary>
        /// Creates a new entry point to the OpenAPI API, handling auth and allowing access to the various API endpoints
        /// </summary>
        /// <param name="openAIAuthentication">
        /// The API authentication information to use for API calls,
        /// or <see langword="null"/> to attempt to use the <see cref="OpenAI.OpenAIAuthentication.Default"/>,
        /// potentially loading from environment vars or from a config file.
        /// </param>
        /// <param name="clientSettings">
        /// Optional, <see cref="OpenAIClientSettings"/> for specifying OpenAI deployments to Azure or proxy domain.
        /// </param>
        /// <exception cref="AuthenticationException">Raised when authentication details are missing or invalid.</exception>
        public OpenAIClient(OpenAIAuthentication? openAIAuthentication = null, OpenAIClientSettings? clientSettings = null)
        {
            OpenAIAuthentication = openAIAuthentication ?? OpenAIAuthentication.Default;
            OpenAIClientSettings = clientSettings ?? OpenAIClientSettings.Default;

            if (OpenAIAuthentication?.ApiKey is null)
            {
                throw new AuthenticationException("You must provide API authentication.  Please refer to https://github.com/RageAgainstThePixel/OpenAI-DotNet#authentication for details.");
            }

            Client = SetupClient();
            JsonSerializationOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                }
            };
            ModelsEndpoint = new ModelsEndpoint(this);
            ChatEndpoint = new ChatEndpoint(this);
        }

        private HttpClient SetupClient(HttpClient? client = null)
        {
            client ??= new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "OpenAI-DotNet");

            if (!OpenAIClientSettings.BaseRequestUrlFormat.Contains(OpenAIClientSettings.AzureOpenAIDomain)
                && (string.IsNullOrWhiteSpace(OpenAIAuthentication.ApiKey) ||
                    (!OpenAIAuthentication.ApiKey.Contains(AuthInfo.SecretKeyPrefix) &&
                     !OpenAIAuthentication.ApiKey.Contains(AuthInfo.SessionKeyPrefix))))
            {
                throw new InvalidCredentialException($"{OpenAIAuthentication.ApiKey} must start with '{AuthInfo.SecretKeyPrefix}'");
            }

            if (OpenAIClientSettings.UseOAuthAuthentication)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", OpenAIAuthentication.ApiKey);
            }
            else
            {
                client.DefaultRequestHeaders.Add("api-key", OpenAIAuthentication.ApiKey);
            }

            if (!string.IsNullOrWhiteSpace(OpenAIAuthentication.OrganizationId))
            {
                client.DefaultRequestHeaders.Add("OpenAI-Organization", OpenAIAuthentication.OrganizationId);
            }

            return client;
        }

        /// <summary>
        /// <see cref="HttpClient"/> to use when making calls to the API.
        /// </summary>
        internal HttpClient Client { get; private set; }

        /// <summary>
        /// The <see cref="JsonSerializationOptions"/> to use when making calls to the API.
        /// </summary>
        internal JsonSerializerOptions JsonSerializationOptions { get; }

        /// <summary>
        /// The API authentication information to use for API calls
        /// </summary>
        public OpenAIAuthentication OpenAIAuthentication { get; }

        /// <summary>
        /// The client settings for configuring Azure OpenAI or custom domain.
        /// </summary>
        internal OpenAIClientSettings OpenAIClientSettings { get; }

        /// <summary>
        /// List and describe the various models available in the API.
        /// You can refer to the Models documentation to understand what <see href="https://beta.openai.com/docs/models"/> are available and the differences between them.<br/>
        /// <see href="https://beta.openai.com/docs/api-reference/models"/>
        /// </summary>
        public ModelsEndpoint ModelsEndpoint { get; }

        /// <summary>
        /// Given a chat conversation, the model will return a chat completion response.<br/>
        /// <see href="https://platform.openai.com/docs/api-reference/chat"/>
        /// </summary>
        public ChatEndpoint ChatEndpoint { get; }
    }
}
