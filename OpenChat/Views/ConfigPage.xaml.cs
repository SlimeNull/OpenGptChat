using CommunityToolkit.Mvvm.Input;
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
        public ConfigPage(ConfigPageModel viewModel, PageService pageService, ConfigurationService configurationService)
        {
            ViewModel = viewModel;
            PageService = pageService;
            ConfigurationService = configurationService;
            InitializeComponent();

            DataContext = this;
        }

        public ConfigPageModel ViewModel { get; }
        public PageService PageService { get; }
        public ConfigurationService ConfigurationService { get; }

        [RelayCommand]
        public void GoToMainPage()
        {
            PageService.Navigate<MainPage>();
        }

        [RelayCommand]
        public void SaveConfiguration()
        {
            ConfigurationService.Save();
            MessageBox.Show(App.Current.MainWindow, "Configuration saved.", "OK", MessageBoxButton.OK, MessageBoxImage.None);
        }
    }
}
