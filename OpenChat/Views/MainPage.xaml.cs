using CommunityToolkit.Mvvm.Input;
using OpenChat.Models;
using OpenChat.Services;
using OpenChat.ViewModels;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpenChat.Views
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage(
            MainPageModel viewModel,
            PageService pageService,
            ChatService chatService,
            ConfigurationService configurationService)
        {
            ViewModel = viewModel;
            PageService = pageService;
            ChatService = chatService;
            ConfigurationService = configurationService;

            InitializeComponent();

            DataContext = this;
        }

        public MainPageModel ViewModel { get; }
        public PageService PageService { get; }
        public ChatService ChatService { get; }
        public ConfigurationService ConfigurationService { get; }

        [RelayCommand]
        public async Task SendAsync()
        {
            if (string.IsNullOrWhiteSpace(ViewModel.InputBoxText))
                return;

            if (!ViewModel.InputBoxAvailable)
                return;

            string input = ViewModel.InputBoxText;
            ViewModel.InputBoxText = string.Empty;

            ViewModel.Messages.Add(
                new Models.ChatMessage()
                {
                    Username = "Me",
                    Message = input,
                });

            bool added = false;
            ChatMessage responseMessage = new ChatMessage()
            {
                Username = "Assistant",
            };

            ViewModel.InputBoxAvailable = false;

            try
            {
                await foreach (var msg in ChatService.Chat(input))
                {
                    responseMessage.Message = msg;

                    if (!added)
                    {
                        ViewModel.Messages.Add(responseMessage);
                        added = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                ViewModel.Messages.Remove(responseMessage);
            }

            ViewModel.InputBoxAvailable = true;
        }

        [RelayCommand]
        public void GoToConfigPage()
        {
            PageService.Navigate<ConfigPage>();
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            //ScrollViewer scrollViewer = (ScrollViewer)sender;

            //scrollViewer.ScrollToEnd();
        }

        bool apikey_notified = false;
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (apikey_notified)
                return;

            if (string.IsNullOrWhiteSpace(ConfigurationService.Instance.ApiKey))
                MessageBox.Show(App.Current.MainWindow, "You can't use OpenChat now, because you haven't set your api key yet", "Warnning", MessageBoxButton.OK, MessageBoxImage.Warning);

            apikey_notified = true;
        }
    }
}
