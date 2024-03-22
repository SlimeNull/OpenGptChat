using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace OpenGptChat.Views.Pages
{
    public partial class ConfigPage : UserControl
    {
        public ConfigPage()
        {
            InitializeComponent();

            
        }

        private void GoToChatPage_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var mainWindow = App.Services.GetRequiredService<MainWindow>();
            mainWindow.Content = App.Services.GetRequiredService<MainPage>();
        }
    }
}
