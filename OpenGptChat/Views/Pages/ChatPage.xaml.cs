    using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using OpenAI.Chat;
using OpenGptChat.Models;
using OpenGptChat.Services;
using OpenGptChat.Utilities;
using OpenGptChat.ViewModels;

namespace OpenGptChat.Views.Pages
{
    /// <summary>
    /// Interaction logic for ChatPage.xaml
    /// </summary>
    public partial class ChatPage : Page
    {
        public ChatPage(
            ChatPageModel viewModel,
            NoteService noteService,
            ChatService chatService,
            ChatStorageService chatStorageService,
            ConfigurationService configurationService,
            SmoothScrollingService smoothScrollingService)
        {
            ViewModel = viewModel;
            NoteService = noteService;
            ChatService = chatService;
            ChatStorageService = chatStorageService;
            ConfigurationService = configurationService;
            DataContext = this;

            InitializeComponent();

            messageScrollViewer.PreviewMouseWheel += CloseAutoScrollWhileMouseWheel;
            messageScrollViewer.ScrollChanged += EnableAutoScrollWhileAtEnd;
            smoothScrollingService.Register(messageScrollViewer);
        }

        public ChatPageModel ViewModel { get; }
        public ChatService ChatService { get; }
        public ChatStorageService ChatStorageService { get; }
        public NoteService NoteService { get; }
        public ConfigurationService ConfigurationService { get; }

        public Guid SessionId { get; private set; }


        public void InitSession(Guid sessionId)
        {
            SessionId = sessionId;

            ViewModel.Messages.Clear();
            foreach (var msg in ChatStorageService.GetAllMessages(SessionId))
                ViewModel.Messages.Add(
                    new ChatMessageModel(msg));
        }



        [RelayCommand]
        public async Task ChatAsync()
        {
            if (string.IsNullOrWhiteSpace(ViewModel.InputBoxText))
            {
                _ = NoteService.ShowAsync("Empty message", 1500);
                return;
            }

            if (string.IsNullOrWhiteSpace(ConfigurationService.Configuration.ApiKey))
            {
                await NoteService.ShowAsync("You can't use OpenChat now, because you haven't set your api key yet", 3000);
                return;
            }


            // 发个消息, 将自动滚动打开, 如果已经在底部, 则将自动滚动打开
            if (messageScrollViewer.IsAtEnd())
                autoScrollToEnd = true;


            string input = ViewModel.InputBoxText.Trim();
            ViewModel.InputBoxText = string.Empty;

            ChatMessageModel requestMessageModel = new ChatMessageModel("user", input);
            ChatMessageModel responseMessageModel = new ChatMessageModel("assistant", string.Empty);

            bool responseAdded = false;

            ViewModel.Messages.Add(requestMessageModel);

            try
            {
                ChatDialogue dialogue =
                    await ChatService.ChatAsync(SessionId, input, content =>
                    {
                        responseMessageModel.Content = content;

                        if (!responseAdded)
                        {
                            responseAdded = true;
                            Dispatcher.Invoke(() =>
                            {
                                ViewModel.Messages.Add(responseMessageModel);
                            });
                        }
                    });

                requestMessageModel.Storage = dialogue.Ask;
                responseMessageModel.Storage = dialogue.Answer;
            }
            catch (TaskCanceledException)
            {
                Rollback(requestMessageModel, responseMessageModel, input);
            }
            catch (Exception ex)
            {
                _ = NoteService.ShowAsync($"{ex.GetType().Name}: {ex.Message}", 3000);

                var chatError = ex.Data.Cast<DictionaryEntry>()
                    .Where(v => v.Key as string is "Error")
                    .Select(v => v.Value as ChatError)
                    .FirstOrDefault();

                if (chatError != null)
                {
                    if (chatError.Code == "context_length_exceeded")
                    {
                        if (ViewModel.Messages.Count > 0)
                        {
                            Console.WriteLine("token reaches the upper limit");
                        }
                    }
                }

                Rollback(requestMessageModel, responseMessageModel, input);
            }

            void Rollback(
                ChatMessageModel requestMessageModel,
                ChatMessageModel responseMessageModel,
                string originInput)
            {
                ViewModel.Messages.Remove(requestMessageModel);
                ViewModel.Messages.Remove(responseMessageModel);

                if (string.IsNullOrWhiteSpace(ViewModel.InputBoxText))
                    ViewModel.InputBoxText = input;
                else
                    ViewModel.InputBoxText = $"{input} {ViewModel.InputBoxText}";
            }
        }

        [RelayCommand]
        public void Cancel()
        {
            ChatService.Cancel();
        }

        [RelayCommand]
        public void ChatOrCancel()
        {
            if (ChatCommand.IsRunning)
                ChatService.Cancel();
            else
                ChatCommand.Execute(null);
        }

        [RelayCommand]
        public void Copy(string text)
        {
            Clipboard.SetText(text);
        }



        bool autoScrollToEnd = false;

        private void CloseAutoScrollWhileMouseWheel(object sender, MouseWheelEventArgs e)
        {
            autoScrollToEnd = false;
        }

        private void EnableAutoScrollWhileAtEnd(object sender, ScrollChangedEventArgs e)
        {
            if (messageScrollViewer.IsAtEnd())
                autoScrollToEnd = true;
        }

        [RelayCommand]
        public void ScrollToEndWhileReceiving()
        {
            if (ChatCommand.IsRunning && autoScrollToEnd)
                messageScrollViewer.ScrollToEnd();
        }
    }
}
