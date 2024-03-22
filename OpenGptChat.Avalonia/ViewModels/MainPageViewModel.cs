using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using OpenGptChat.Models;
using OpenGptChat.Services;
using OpenGptChat.Strings;

namespace OpenGptChat.ViewModels
{
    public partial class MainPageViewModel : ViewModelBase
    {
        private readonly ChatService _chatService = 
            App.Services.GetRequiredService<ChatService>();

        public ObservableCollection<ChatSession> Sessions { get; } = new();

        [ObservableProperty]
        private ChatSession? _selectedSession;

        [ObservableProperty]
        private string _textInput = string.Empty;

        [RelayCommand]
        public async Task Send()
        {
            if (SelectedSession is not ChatSession selectedSession)
                return;
            if (string.IsNullOrWhiteSpace(TextInput))
                return;

            string textInput = TextInput;

            var userMessage = new ChatMessage()
            {
                Role = Role.User,
                Sender = Environment.UserName,
                MessageText = TextInput,
            };

            var assistantMessage = new ChatMessage()
            {
                Role = Role.Assistant,
                Sender = nameof(Role.Assistant)
            };

            try
            {
                TextInput = string.Empty;
                selectedSession.Messages.Add(userMessage);
                selectedSession.Messages.Add(assistantMessage);

                var responseText = 
                    await _chatService.ChatAsync(selectedSession, TextInput, responseText =>
                    {
                        assistantMessage.MessageText = responseText;
                    }, default);

                assistantMessage.MessageText = responseText;
            }
            catch (Exception ex)
            {
                TextInput = textInput;
                selectedSession.Messages.Remove(userMessage);
                selectedSession.Messages.Remove(assistantMessage);
            }
        }

        [RelayCommand]
        public void CreateNewSession()
        {
            var newSession =
                new ChatSession()
                {
                    Title = "New session"
                };

            Sessions.Add(newSession);
            SelectedSession = newSession;
        }
    }
}
