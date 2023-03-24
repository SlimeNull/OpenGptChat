using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Models;
using OpenChat.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenChat.Services
{
    public class ChatService
    {
        public ChatService(ConfigurationService configurationService)
        {
            ConfigurationService = configurationService;
        }

        private OpenAIClient? client;
        private string? client_apikey;
        private string? client_apihost;

        private readonly List<ChatPrompt> chatHistory =
            new List<ChatPrompt>();




        public ConfigurationService ConfigurationService { get; }

        private void NewOpenAIClient(
            [NotNull] out OpenAIClient client, 
            [NotNull] out string client_apikey,
            [NotNull] out string client_apihost)
        {
            client_apikey = ConfigurationService.Configuration.ApiKey;
            client_apihost = ConfigurationService.Configuration.ApiHost;

            client = new OpenAIClient(
                new OpenAIAuthentication(ConfigurationService.Configuration.ApiKey),
                new OpenAIClientSettings(ConfigurationService.Configuration.ApiHost));
        }

        private OpenAIClient GetOpenAIClient()
        {
            if (client == null ||
                client_apikey != ConfigurationService.Configuration.ApiKey ||
                client_apihost != ConfigurationService.Configuration.ApiHost)
                NewOpenAIClient(out client, out client_apikey, out client_apihost);

            return client;
        }

        public async Task Chat(string message, Action<string> messageHandler)
        {
            OpenAIClient client = GetOpenAIClient();

            List<ChatPrompt> messages = new List<ChatPrompt>();

            foreach (var sysmsg in ConfigurationService.Configuration.SystemMessages)
                messages.Add(new ChatPrompt("system", sysmsg));

            foreach (var chatmsg in chatHistory)
                messages.Add(chatmsg);

            messages.Add(new ChatPrompt("user", message));

            string modelName =
                ConfigurationService.Configuration.ApiGptModel;

            DateTime lastTime = DateTime.Now;

            StringBuilder sb = new StringBuilder();

            CancellationTokenSource cancelTaskCancellation = new CancellationTokenSource();
            CancellationTokenSource completionTaskCancellation = new CancellationTokenSource();

            Task completionTask = client.ChatEndpoint.StreamCompletionAsync(
                new ChatRequest(messages, modelName),
                response =>
                {
                    string? content = response.Choices.FirstOrDefault()?.Delta?.Content;
                    if (!string.IsNullOrEmpty(content))
                    {
                        sb.Append(content);

                        while (sb.Length > 0 && char.IsWhiteSpace(sb[0]))
                            sb.Remove(0, 1);

                        messageHandler.Invoke(sb.ToString());

                        // 有响应了, 更新时间
                        lastTime = DateTime.Now;
                    }
                }, completionTaskCancellation.Token);

            Task cancelTask = Task.Run(async () =>
            {
                CancellationToken cancellationToken = cancelTaskCancellation.Token;

                try
                {
                    TimeSpan timeout = 
                        TimeSpan.FromMilliseconds(ConfigurationService.Configuration.ApiTimeout);

                    while (!completionTask.IsCompleted)
                    {
                        await Task.Delay(100);

                        if (cancellationToken.IsCancellationRequested)
                            return;

                        // 如果当前时间与上次响应的时间相差超过配置的超时时间, 则扔异常
                        if ((DateTime.Now - lastTime) > timeout)
                        {
                            completionTaskCancellation.Cancel();
                            throw new TimeoutException();
                        }
                    }
                }
                catch (TaskCanceledException)
                {
                    return;
                }
            });

            await Task.WhenAll(completionTask, cancelTask);

            chatHistory.Add(new ChatPrompt("user", message));
            chatHistory.Add(new ChatPrompt("assistant", sb.ToString()));
        }

        public void Clear()
        {
            chatHistory.Clear();
        }
    }
}
