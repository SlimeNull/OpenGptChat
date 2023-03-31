using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;
using OpenGptChat.Abstraction;
using OpenGptChat.Models;
using OpenGptChat.Services;
using OpenGptChat.ViewModels;

namespace OpenGptChat.Views
{
    /// <summary>
    /// Interaction logic for ConfigPage.xaml
    /// </summary>
    public partial class ConfigPage : Page, IConfigPage
    {
        public ConfigPage(
            IAppWindow appWindow,
            ConfigPageModel viewModel,
            PageService pageService,
            NoteService noteService,
            LanguageService languageService,
            ColorModeService colorModeService,
            ConfigurationService configurationService,
            SmoothScrollingService smoothScrollingService)
        {
            AppWindow = appWindow;
            ViewModel = viewModel;
            PageService = pageService;
            NoteService = noteService;
            LanguageService = languageService;
            ColorModeService = colorModeService;
            ConfigurationService = configurationService;
            DataContext = this;

            LoadSystemMessagesCore();

            InitializeComponent();

            smoothScrollingService.Register(configurationScrollViewer);
        }

        public IAppWindow AppWindow { get; }
        public ConfigPageModel ViewModel { get; }
        public PageService PageService { get; }
        public NoteService NoteService { get; }
        public LanguageService LanguageService { get; }
        public ColorModeService ColorModeService { get; }
        public ConfigurationService ConfigurationService { get; }

        private void LoadSystemMessagesCore()
        {
            ViewModel.SystemMessages.Clear();
            foreach (var msg in ConfigurationService.Configuration.SystemMessages)
                ViewModel.SystemMessages.Add(new ValueWrapper<string>(msg));
        }

        private void ApplySystemMessagesCore()
        {
            ConfigurationService.Configuration.SystemMessages = ViewModel.SystemMessages
                .Select(wraper => wraper.Value)
                .ToArray();
        }


        [RelayCommand]
        public void GoToMainPage()
        {
            AppWindow.Navigate<IMainPage>();
        }

        [RelayCommand]
        public void AboutOpenChat()
        {
            MessageBox.Show(App.Current.MainWindow,
                $"""
                {nameof(OpenGptChat)}, by SlimeNull v{Assembly.GetEntryAssembly()?.GetName()?.Version}

                A simple chat client based on OpenAI Chat completion API.

                Repository: https://github.com/SlimeNull/{nameof(OpenGptChat)}
                """,
                $"About {nameof(OpenGptChat)}", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        [RelayCommand]
        public Task LoadSystemMessages()
        {
            LoadSystemMessagesCore();
            return NoteService.ShowAsync("System messages loaded", 1500);
        }

        [RelayCommand]
        public Task ApplySystemMessages()
        {
            ApplySystemMessagesCore();
            return NoteService.ShowAsync("System messages applied", 1500);
        }

        [RelayCommand]
        public void AddSystemMessage()
        {
            ViewModel.SystemMessages.Add(new ValueWrapper<string>("New system message"));
        }

        [RelayCommand]
        public void RemoveSystemMessage()
        {
            if (ViewModel.SystemMessages.Count > 0)
            {
                ViewModel.SystemMessages.RemoveAt(ViewModel.SystemMessages.Count - 1);
            }
        }

        [RelayCommand]
        public Task SaveConfiguration()
        {
            ConfigurationService.Configuration.Language =
                LanguageService.CurrentLanguage.ToString();
            ConfigurationService.Configuration.ColorMode =
                ColorModeService.CurrentMode;

            ConfigurationService.Save();
            return NoteService.ShowAsync("Configuration saved", 2000);
        }
    }
}
