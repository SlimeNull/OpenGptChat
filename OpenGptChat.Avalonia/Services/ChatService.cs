using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenAI;
using OpenAI.Chat;
using OpenGptChat.Models;

namespace OpenGptChat.Services
{
    public class ChatService
    {
        private readonly ConfigService _configService;

        private OpenAIClient? _client;
        private string? _currentClientAPIKey;
        private string? _currentClientAPIHost;
        private string? _currentClientOrganization;

        public ChatService(
            ConfigService configService)
        {
            _configService = configService;
        }

        [MemberNotNullWhen(false, nameof(_client))]
        private bool ShouldCreateNewClient()
        {
            return
                _client == null ||
                _currentClientAPIKey != _configService.AppConfig.ApiKey ||
                _currentClientAPIHost != _configService.AppConfig.ApiHost ||
                _currentClientOrganization != _configService.AppConfig.Organization;
        }

        private OpenAIClient CreateNewClient()
        {
            OpenAIClient client = new(
                new OpenAIAuthentication(
                    _currentClientAPIKey = _configService.AppConfig.ApiKey,
                    _currentClientOrganization = _configService.AppConfig.Organization),
                new OpenAIClientSettings(
                    _currentClientAPIHost = _configService.AppConfig.ApiHost));

            return client;
        }

        private IEnumerable<Message> BuildMessages(
            ChatSession session,
            string message)
        {
            foreach (var systemMessage in _configService.AppConfig.SystemMessages)
                yield return new Message(OpenAI.Role.System, systemMessage);
            foreach (var sessionSystemMessage in session.SystemMessages)
                yield return new Message(OpenAI.Role.System, sessionSystemMessage.MessageText, sessionSystemMessage.Sender);
            foreach (var sessionMessage in session.Messages)
                yield return ConvertMessage(sessionMessage);

            yield return new Message(OpenAI.Role.User, message);

            Message ConvertMessage(ChatMessage message)
            {
                return new Message(
                    role: message.Role switch
                    {
                        Models.Role.User => OpenAI.Role.User,
                        Models.Role.Assistant => OpenAI.Role.Assistant,
                        Models.Role.System => OpenAI.Role.System,
                        _ => OpenAI.Role.Assistant
                    },
                    content: message.MessageText);
            }
        }

        public async Task<string> ChatAsync(
            ChatSession session,
            string message,
            Action<string> responseHandler,
            CancellationToken cancellationToken)
        {
            if (ShouldCreateNewClient())
                _client = CreateNewClient();

            ChatRequest request = new ChatRequest(
                messages: BuildMessages(session, message),
                model: _configService.AppConfig.Model,
                temperature: _configService.AppConfig.Temerature);

            DateTime lastTime = DateTime.Now;
            CancellationTokenSource chatTaskCancellation =
                CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            StringBuilder resultBuilder = new();

            Task chatTask = _client.ChatEndpoint.StreamCompletionAsync(
                request,
                response =>
                {
                    string? content = response.Choices.FirstOrDefault()?.Delta?.Content;

                    if (string.IsNullOrEmpty(content))
                        return;

                    resultBuilder.Append(content);
                    responseHandler.Invoke(resultBuilder.ToString());

                    lastTime = DateTime.Now;
                },
                cancellationToken);

            Task cancelTask = Task.Run(async() =>
            {
                try
                {
                    TimeSpan timeout = TimeSpan.FromMilliseconds(
                        _configService.AppConfig.ApiTimeout);

                    if (timeout.Ticks <= 0)
                        return;

                    while (!chatTask.IsCompleted)
                    {
                        await Task.Delay(timeout / 10);

                        if ((DateTime.Now - lastTime) > timeout)
                        {
                            chatTaskCancellation.Cancel();
                            throw new TimeoutException();
                        }
                    }
                }
                catch(TaskCanceledException)
                {
                    return;
                }
            });

            await Task.WhenAll(chatTask, cancelTask);

            return resultBuilder.ToString();
        }
    }
}
