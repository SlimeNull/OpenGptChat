using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;
using OpenGptChat.Models;
using OpenGptChat.Services;
using OpenGptChat.ViewModels;

namespace OpenGptChat.Views.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage(
            AppWindow appWindow,
            MainPageModel viewModel,
            AppGlobalData appGlobalData,
            PageService pageService,
            NoteService noteService,
            ChatService chatService,
            ChatPageService chatPageService,
            ChatStorageService chatStorageService,
            ConfigurationService configurationService,
            SmoothScrollingService smoothScrollingService)
        {
            AppWindow = appWindow;
            ViewModel = viewModel;
            AppGlobalData = appGlobalData;
            PageService = pageService;
            NoteService = noteService;
            ChatService = chatService;
            ChatPageService = chatPageService;
            ChatStorageService = chatStorageService;
            ConfigurationService = configurationService;
            DataContext = this;

            // 从存储中将所有会话添加进去
            foreach (var session in ChatStorageService.GetAllSessions())
                AppGlobalData.Sessions.Add(new ChatSessionModel(session));

            // 如果没有会话, 则创建一个
            if (AppGlobalData.Sessions.Count == 0)
                NewSession();

            InitializeComponent();

            // 切换到指定页面
            SwitchPageToCurrentSession();

            // 为滚动查看器注册平滑滚动
            smoothScrollingService.Register(sessionsScrollViewer);
        }

        public AppWindow AppWindow { get; }
        public MainPageModel ViewModel { get; }
        public AppGlobalData AppGlobalData { get; }
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
            AppWindow.Navigate<ConfigPage>();
        }

        /// <summary>
        /// 重置聊天
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        public async Task ResetChat()
        {
            if (AppGlobalData.SelectedSession != null)
            {
                Guid sessionId = AppGlobalData.SelectedSession.Id;

                ChatService.Cancel();
                ChatStorageService.ClearMessage(sessionId);
                ViewModel.CurrentChat?.ViewModel.Messages.Clear();

                await NoteService.ShowAndWaitAsync("Chat has been reset.", 1500);
            }
            else
            {
                await NoteService.ShowAndWaitAsync("You need to select a session.", 1500);
            }
        }

        /// <summary>
        /// 新建会话
        /// </summary>
        [RelayCommand]
        public void NewSession()
        {
            ChatSession session = ChatSession.Create();
            ChatSessionModel sessionModel = new ChatSessionModel(session);

            ChatStorageService.SaveSession(session);
            AppGlobalData.Sessions.Add(sessionModel);

            AppGlobalData.SelectedSession = sessionModel;
        }

        /// <summary>
        /// 删除会话
        /// </summary>
        /// <param name="session"></param>
        [RelayCommand]
        public void DeleteSession(ChatSessionModel session)
        {
            if (AppGlobalData.Sessions.Count == 1)
            {
                NoteService.Show("You can't delete the last session.", 1500);
                return;
            }

            int index = 
                AppGlobalData.Sessions.IndexOf(session);
            int newIndex =
                Math.Max(0, index - 1);

            ChatPageService.RemovePage(session.Id);
            ChatStorageService.DeleteSession(session.Id);
            AppGlobalData.Sessions.Remove(session);

            AppGlobalData.SelectedSession = AppGlobalData.Sessions[newIndex];
        }

        [RelayCommand]
        public void SwitchToNextSession()
        {
            int nextIndex;
            int lastIndex = AppGlobalData.Sessions.Count - 1;

            if (AppGlobalData.SelectedSession != null)
                nextIndex = AppGlobalData.Sessions.IndexOf(AppGlobalData.SelectedSession) + 1;
            else
                nextIndex = 0;

            nextIndex = Math.Clamp(nextIndex, 0, lastIndex);

            AppGlobalData.SelectedSession = 
                AppGlobalData.Sessions[nextIndex];
        }

        [RelayCommand]
        public void SwitchToPreviousSession()
        {
            int previousIndex;
            int lastIndex = AppGlobalData.Sessions.Count - 1;

            if (AppGlobalData.SelectedSession != null)
                previousIndex = AppGlobalData.Sessions.IndexOf(AppGlobalData.SelectedSession) - 1;
            else
                previousIndex = 0;

            previousIndex = Math.Clamp(previousIndex, 0, lastIndex);

            AppGlobalData.SelectedSession =
                AppGlobalData.Sessions[previousIndex];
        }


        [RelayCommand]
        public void CycleSwitchToNextSession()
        {
            int nextIndex;
            int lastIndex = AppGlobalData.Sessions.Count - 1;

            if (AppGlobalData.SelectedSession != null)
                nextIndex = AppGlobalData.Sessions.IndexOf(AppGlobalData.SelectedSession) + 1;
            else
                nextIndex = 0;

            if (nextIndex > lastIndex)
                nextIndex = 0;

            AppGlobalData.SelectedSession =
                AppGlobalData.Sessions[nextIndex];
        }

        [RelayCommand]
        public void CycleSwitchToPreviousSession()
        {
            int previousIndex;
            int lastIndex = AppGlobalData.Sessions.Count - 1;

            if (AppGlobalData.SelectedSession != null)
                previousIndex = AppGlobalData.Sessions.IndexOf(AppGlobalData.SelectedSession) - 1;
            else
                previousIndex = 0;

            if (previousIndex < 0)
                previousIndex = lastIndex;

            AppGlobalData.SelectedSession =
                AppGlobalData.Sessions[previousIndex];
        }

        [RelayCommand]
        public void DeleteCurrentSession()
        {
            if (AppGlobalData.SelectedSession != null)
                DeleteSession(AppGlobalData.SelectedSession);
        }


        [RelayCommand]
        public void SwitchPageToCurrentSession()
        {
            if (AppGlobalData.SelectedSession != null)
            {
                // 将页面中当前显示的聊天页面切换到对应页面
                ViewModel.CurrentChat =
                    ChatPageService.GetPage(AppGlobalData.SelectedSession.Id);
            }
        }
    }
}
