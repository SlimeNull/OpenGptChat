using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
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
            AppGlobalData appGlobalData,
            NoteService noteService,
            ChatService chatService,
            ChatStorageService chatStorageService,
            ConfigurationService configurationService,
            SmoothScrollingService smoothScrollingService,
            TitleGenerationService titleGenerationService)
        {
            ViewModel = viewModel;
            AppGlobalData = appGlobalData;
            NoteService = noteService;
            ChatService = chatService;
            ChatStorageService = chatStorageService;
            ConfigurationService = configurationService;
            TitleGenerationService = titleGenerationService;
            DataContext = this;

            InitializeComponent();

            messagesScrollViewer.PreviewMouseWheel += CloseAutoScrollWhileMouseWheel;
            messagesScrollViewer.ScrollChanged += MessageScrolled;

            smoothScrollingService.Register(messagesScrollViewer);
        }

        private ChatSessionModel? currentSessionModel;

        public ChatPageModel ViewModel { get; }
        public AppGlobalData AppGlobalData { get; }
        public ChatService ChatService { get; }
        public ChatStorageService ChatStorageService { get; }
        public NoteService NoteService { get; }
        public ConfigurationService ConfigurationService { get; }
        public TitleGenerationService TitleGenerationService { get; }
        public Guid SessionId { get; private set; }
        public ChatSessionModel? CurrentSessionModel => currentSessionModel ??= AppGlobalData.Sessions.FirstOrDefault(session => session.Id == SessionId);


        public void InitSession(Guid sessionId)
        {
            SessionId = sessionId;

            ViewModel.Messages.Clear();
            foreach (var msg in ChatStorageService.GetLastMessages(SessionId, 10))
                ViewModel.Messages.Add(new ChatMessageModel(msg));
        }



        [RelayCommand]
        public async Task ChatAsync()
        {
            if (string.IsNullOrWhiteSpace(ViewModel.InputBoxText))
            {
                _ = NoteService.ShowAndWaitAsync("Empty message", 1500);
                return;
            }

            if (string.IsNullOrWhiteSpace(ConfigurationService.Configuration.ApiKey))
            {
                await NoteService.ShowAndWaitAsync("You can't use OpenChat now, because you haven't set your api key yet", 3000);
                return;
            }


            // 发个消息, 将自动滚动打开, 如果已经在底部, 则将自动滚动打开
            if (messagesScrollViewer.IsAtEnd())
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

                if (CurrentSessionModel is ChatSessionModel currentSessionModel &&
                    string.IsNullOrEmpty(currentSessionModel.Name))
                {
                    string? title = await TitleGenerationService.GenerateAsync(new []
                    {
                        requestMessageModel.Content,
                        responseMessageModel.Content
                    });

                    currentSessionModel.Name = title;
                }
            }
            catch (TaskCanceledException)
            {
                Rollback(requestMessageModel, responseMessageModel, input);
            }
            catch (Exception ex)
            {
                _ = NoteService.ShowAndWaitAsync($"{ex.GetType().Name}: {ex.Message}", 3000);

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

        private void MessageScrolled(object sender, ScrollChangedEventArgs e)
        {
            if (e.OriginalSource != messagesScrollViewer)
                return;

            if (messagesScrollViewer.IsAtEnd())
                autoScrollToEnd = true;

            if (e.VerticalChange != 0 && 
                messages.IsLoaded && IsLoaded &&
                messagesScrollViewer.IsAtTop(10) &&
                ViewModel.Messages.FirstOrDefault()?.Storage?.Timestamp is DateTime timestamp)
            {
                foreach (var msg in ChatStorageService.GetLastMessagesBefore(SessionId, 10, timestamp))
                    ViewModel.Messages.Insert(0, new ChatMessageModel(msg));

                double distanceFromEnd = messagesScrollViewer.ScrollableHeight - messagesScrollViewer.VerticalOffset;
                Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action<ScrollChangedEventArgs>(e =>
                {
                    ScrollViewer sv = (ScrollViewer)e.Source;
                    sv.ScrollToVerticalOffset(sv.ScrollableHeight - distanceFromEnd);
                }), e);

                e.Handled = true;
            }
        }

        [RelayCommand]
        public void ScrollToEndWhileReceiving()
        {
            if (ChatCommand.IsRunning && autoScrollToEnd)
                messagesScrollViewer.ScrollToEnd();
        }
    }
}
