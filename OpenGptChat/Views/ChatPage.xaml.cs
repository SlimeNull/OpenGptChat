using CommunityToolkit.Mvvm.Input;
using OpenGptChat.Abstraction;
using OpenGptChat.Models;
using OpenGptChat.Services;
using OpenGptChat.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpenGptChat.Views
{
    /// <summary>
    /// Interaction logic for ChatPage.xaml
    /// </summary>
    public partial class ChatPage : Page, IChatPage
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

            foreach (var msg in ChatStorageService.GetAllMessages(SessionId))
                ViewModel.Messages.Add(
                    new ChatMessageModel(msg));
        }



        [RelayCommand]
        public async Task SendAsync()
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
            catch (Exception ex)
            {
                _ = NoteService.ShowAsync($"{ex.GetType().Name}: {ex.Message}", 3000);
                ViewModel.Messages.Remove(requestMessageModel);
                ViewModel.Messages.Remove(responseMessageModel);

                ViewModel.InputBoxText = input;
            }
        }

        [RelayCommand]
        public void Copy(string text)
        {
            Clipboard.SetText(text);
        }

        [RelayCommand]
        public void ScrollToEndWhileReceiving()
        {
            if (SendCommand.IsRunning)
                messageScrollViewer.ScrollToEnd();
        }
    }
}
