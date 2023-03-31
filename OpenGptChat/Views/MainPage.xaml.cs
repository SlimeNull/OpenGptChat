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

            // 从存储中将所有会话添加进去
            foreach (var session in ChatStorageService.GetAllSessions())
                ViewModel.Sessions.Add(new ChatSessionModel(session));

            // 如果没有会话, 则创建一个
            if (ViewModel.Sessions.Count == 0)
                NewSession();

            InitializeComponent();

            // 切换到指定页面
            SwitchPageToCurrentSession();

            // 为滚动查看器注册平滑滚动
            smoothScrollingService.Register(sessionsScrollViewer);
        }

        public MainPageModel ViewModel { get; }
        public PageService PageService { get; }
        public NoteService NoteService { get; }
        public ChatService ChatService { get; }
        public ChatPageService ChatPageService { get; }
        public ChatStorageService ChatStorageService { get; }
        public ConfigurationService ConfigurationService { get; }



        /// <summary>
        /// 转到配置页面
        /// </summary>
        [RelayCommand]
        public void GoToConfigPage()
        {
            PageService.Navigate<ConfigPage>();
        }

        /// <summary>
        /// 重置聊天
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 新建会话
        /// </summary>
        [RelayCommand]
        public void NewSession()
        {
            ChatSession session = ChatSession.Create("New session");
            ChatSessionModel sessionModel = new ChatSessionModel(session);

            ChatStorageService.SaveSession(session);
            ViewModel.Sessions.Add(sessionModel);
        }

        /// <summary>
        /// 删除会话
        /// </summary>
        /// <param name="session"></param>
        [RelayCommand]
        public void DeleteSession(ChatSessionModel session)
        {
            ChatPageService.RemovePage(session.Id);
            ChatStorageService.DeleteSession(session.Id);
            ViewModel.Sessions.Remove(session);
        }


        [RelayCommand]
        public void SwitchPageToCurrentSession()
        {
            if (ViewModel.SelectedSession != null)
            {
                // 将页面中当前显示的聊天页面切换到对应页面
                ViewModel.CurrentChat =
                    ChatPageService.GetPage(ViewModel.SelectedSession.Id);
            }
        }
    }
}
