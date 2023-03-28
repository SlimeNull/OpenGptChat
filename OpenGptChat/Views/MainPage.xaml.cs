using CommunityToolkit.Mvvm.Input;
using OpenGptChat.Models;
using OpenGptChat.Services;
using OpenGptChat.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
            ChatPageService chatPageService,
            ChatStorageService chatStorageService,
            ConfigurationService configurationService,
            SmoothScrollingService smoothScrollingService)
        {
            ViewModel = viewModel;
            PageService = pageService;
            NoteService = noteService;
            ChatService = chatService;
            ChatPageService = chatPageService;
            ChatStorageService = chatStorageService;
            ConfigurationService = configurationService;
            DataContext = this;

            foreach (var session in ChatStorageService.GetAllSessions())
                ViewModel.Sessions.Add(new ChatSessionModel(session));

#if DEBUG
            if (ViewModel.Sessions.Count == 0)
                ViewModel.Sessions.Add(new ChatSessionModel(ChatService.NewSession("New session")));
#endif
            InitializeComponent();

            smoothScrollingService.Register(sessionsScrollViewer);
        }

        public MainPageModel ViewModel { get; }
        public PageService PageService { get; }
        public NoteService NoteService { get; }
        public ChatService ChatService { get; }
        public ChatPageService ChatPageService { get; }
        public ChatStorageService ChatStorageService { get; }
        public ConfigurationService ConfigurationService { get; }



        [RelayCommand]
        public void GoToConfigPage()
        {
            PageService.Navigate<ConfigPage>();
        }

        [RelayCommand]
        public async Task ResetChat()
        {
            if (ViewModel.SelectedSession != null)
            {
                Guid sessionId = ViewModel.SelectedSession.Id;

                ChatService.Cancel();
                ChatStorageService.ClearMessage(sessionId);
                ViewModel.CurrentChat?.ViewModel.Messages.Clear();

                await NoteService.ShowAsync("Chat has been reset.", 1500);
            }
            else
            {
                await NoteService.ShowAsync("You need to select a session.", 1500);
            }
        }

        [RelayCommand]
        public void NewSession()
        {
            ChatSession session = ChatSession.Create("New session");
            ChatSessionModel sessionModel = new ChatSessionModel(session);

            ChatStorageService.SaveSession(session);
            ViewModel.Sessions.Add(sessionModel);
        }

        [RelayCommand]
        public void DeleteSession(ChatSessionModel session)
        {
            ChatStorageService.DeleteSession(session.Id);
            ViewModel.Sessions.Remove(session);
        }

        private void ChatSessions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.SelectedSession != null)
            {
                ViewModel.CurrentChat = ChatPageService.GetPage(ViewModel.SelectedSession.Id);
            }
        }


        /// <summary>
        /// 实在没办法绑定了, 只能订阅事件了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_DeleteSession_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem item && item.CommandParameter is ChatSessionModel session)
                DeleteSession(session);
        }
    }
}
