using CommunityToolkit.Mvvm.Input;
using OpenChat.Models;
using OpenChat.Services;
using OpenChat.ViewModels;
using System;
using System.Collections.Generic;
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

namespace OpenChat.Views
{
    /// <summary>
    /// Interaction logic for ConfigPage.xaml
    /// </summary>
    public partial class ConfigPage : Page
    {
        public ConfigPage(
            ConfigPageModel viewModel,
            PageService pageService,
            NoteService noteService,
            ConfigurationService configurationService,
            SmoothScrollingService smoothScrollingService)
        {
            ViewModel = viewModel;
            PageService = pageService;
            NoteService = noteService;
            ConfigurationService = configurationService;
            InitializeComponent();

            DataContext = this;

            smoothScrollingService.Register(configurationScrollViewer);
        }

        public ConfigPageModel ViewModel { get; }
        public PageService PageService { get; }
        public NoteService NoteService { get; }
        public ConfigurationService ConfigurationService { get; }

        [RelayCommand]
        public void GoToMainPage()
        {
            PageService.Navigate<MainPage>();
        }

        [RelayCommand]
        public void AboutOpenChat()
        {
            MessageBox.Show(App.Current.MainWindow,
                """
                OpenChat, by SlimeNull

                A simple chat client based on OpenAI Chat completion API.

                Repository: https://github.com/SlimeNull/OpenChat
                """,
                "About OpenChat", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        [RelayCommand]
        public Task ApplySystemMessages()
        {
            ViewModel.ApplySystemMessages();
            return NoteService.ShowAsync("System messages applied", 1500);
            //MessageBox.Show(App.Current.MainWindow, "System messages applied.", "OK", MessageBoxButton.OK, MessageBoxImage.None);
        }

        [RelayCommand]
        public void AddSystemMessage()
        {
            ViewModel.SystemMessages.Add(new ValueWrapper<string>("New system message"));
        }

        [RelayCommand]
        public void RemoveSystemMessage()
        {
            ViewModel.SystemMessages.RemoveAt(ViewModel.SystemMessages.Count - 1);
        }

        [RelayCommand]
        public Task SaveConfiguration()
        {
            ConfigurationService.Save();
            return NoteService.ShowAsync("Configuration saved", 2000);
            //MessageBox.Show(App.Current.MainWindow, "Configuration saved.", "OK", MessageBoxButton.OK, MessageBoxImage.None);
        }
    }
}
