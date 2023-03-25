using CommunityToolkit.Mvvm.Input;
using OpenGptChat.Services;
using OpenGptChat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpenGptChat.Views
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage(
            MainPageModel viewModel,
            PageService pageService,
            NoteService noteService,
            ChatService chatService,
            ConfigurationService configurationService,
            SmoothScrollingService smoothScrollingService)
        {
            ViewModel = viewModel;
            PageService = pageService;
            NoteService = noteService;
            ChatService = chatService;
            ConfigurationService = configurationService;

            InitializeComponent();

            DataContext = this;

            smoothScrollingService.Register(messageScrollViewer);
        }

        public MainPageModel ViewModel { get; }
        public PageService PageService { get; }
        public NoteService NoteService { get; }
        public ChatService ChatService { get; }
        public ConfigurationService ConfigurationService { get; }

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

            ChatMessage requestMessage = new ChatMessage()
            {
                Username = "Me",
                Message = input,
            };

            ChatMessage responseMessage = new ChatMessage()
            {
                Username = "Assistant",
            };

            bool responseAdded = false;

            ViewModel.Messages.Add(requestMessage);

            try
            {
                await ChatService.ChatAsync(input, message =>
                {
                    responseMessage.Message = message;

                    if (!responseAdded)
                    {
                        ViewModel.Messages.Add(responseMessage);
                        responseAdded = true;
                    }
                });
            }
            catch (Exception ex)
            {
                _ =  NoteService.ShowAsync($"{ex.GetType().Name}: {ex.Message}", 3000);
                ViewModel.Messages.Remove(requestMessage);
                ViewModel.Messages.Remove(responseMessage);

                ViewModel.InputBoxText = input;
            }
        }

        [RelayCommand]
        public void GoToConfigPage()
        {
            PageService.Navigate<ConfigPage>();
        }

        [RelayCommand]
        public async Task ResetChat()
        {
            ViewModel.Messages.Clear();
            ChatService.Cancel();
            ChatService.Clear();
            await NoteService.ShowAsync("Chat has been reset.", 1500);
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer scrollViewer = (ScrollViewer)sender;

            if (SendCommand.IsRunning)
                scrollViewer.ScrollToEnd();
        }
    }
}
