using Microsoft.Extensions.Options;
using OpenAI_API;
using OpenChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenChat.Services
{
    public class ChatService
    {
        public ChatService(IOptions<AppConfig> optionsSnapshot)
        {
            Configuration = optionsSnapshot;
        }

        private readonly List<OpenAI_API.Chat.ChatMessage> chatHistory =
            new List<OpenAI_API.Chat.ChatMessage>();
        public IOptions<AppConfig> Configuration { get; }

        public async IAsyncEnumerable<string> Chat(string message)
        {
            OpenAIAPI api;

            api = new OpenAIAPI(Configuration.Value.ApiKey);
            api.ApiUrlFormat = $"https://{Configuration.Value.ApiHost}/{{0}}/{{1}}";

            List<OpenAI_API.Chat.ChatMessage> messages = new List<OpenAI_API.Chat.ChatMessage>();

            foreach (var sysmsg in Configuration.Value.SystemMessages)
                messages.Add(
                    new OpenAI_API.Chat.ChatMessage()
                    {
                        Role = OpenAI_API.Chat.ChatMessageRole.System,
                        Content = sysmsg
                    });

            foreach (var chatmsg in chatHistory)
                messages.Add(chatmsg);

            messages.Add(new OpenAI_API.Chat.ChatMessage()
            {
                Role = OpenAI_API.Chat.ChatMessageRole.User,
                Content = message
            });

            var conversation = api.Chat.StreamChatEnumerableAsync(
                new OpenAI_API.Chat.ChatRequest()
                {
                    Model = Configuration.Value.ApiGptModel,
                    Messages = messages
                });

            StringBuilder sb = new StringBuilder();
            await foreach (var xxx in conversation)
            {
                string? content = xxx.Choices.FirstOrDefault()?.Delta?.Content;
                if (!string.IsNullOrEmpty(content))
                {
                    sb.Append(content);

                    while (sb.Length > 0 && char.IsWhiteSpace(sb[0]))
                        sb.Remove(0, 1);

                    yield return sb.ToString();
                }
            }

            chatHistory.Add(new OpenAI_API.Chat.ChatMessage()
            {
                Role = OpenAI_API.Chat.ChatMessageRole.User,
                Content = message,
            });

            chatHistory.Add(new OpenAI_API.Chat.ChatMessage()
            {
                Role = OpenAI_API.Chat.ChatMessageRole.Assistant,
                Content = sb.ToString(),
            });
        }

        public void Clear()
        {
            chatHistory.Clear();
        }
    }
}
