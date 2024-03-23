using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using OpenGptChat.Services;
using OpenGptChat.ViewModels;

namespace OpenGptChat.Views.Pages
{
    public partial class ConfigPage : UserControl
    {
        public ConfigPage()
        {
            DataContext = new ConfigPageViewModel();

            InitializeComponent();
        }

        private void PageLostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            App.Services
                .GetRequiredService<ConfigService>()
                .Save();
        }

        private void PageLoaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var config = App.Services
                .GetRequiredService<ConfigService>()
                .AppConfig;

            if (string.IsNullOrWhiteSpace(config.ApiKey) ||
                string.IsNullOrWhiteSpace(config.ApiHost))
                APISettings.IsExpanded = true;
        }
    }
}
