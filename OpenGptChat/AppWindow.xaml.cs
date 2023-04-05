using System;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using OpenGptChat.Services;
using OpenGptChat.ViewModels;

namespace OpenGptChat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AppWindow : Window
    {
        public AppWindow(
            AppWindowModel viewModel,
            PageService pageService,
            NoteService noteService,
            LanguageService languageService,
            ColorModeService colorModeService)
        {
            ViewModel = viewModel;
            PageService = pageService;
            NoteService = noteService;
            LanguageService = languageService;
            ColorModeService = colorModeService;
            DataContext = this;

            InitializeComponent();
        }

        public AppWindowModel ViewModel { get; }
        public PageService PageService { get; }
        public NoteService NoteService { get; }
        public LanguageService LanguageService { get; }
        public ColorModeService ColorModeService { get; }


        public void Navigate<TPage>() where TPage : class
        {
            appFrame.Navigate(
                PageService.GetPage<TPage>());
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            // 窗体句柄创建啦, 我可以搞事情辣!
            LanguageService.Init();
            ColorModeService.Init();
        }
    }
}
