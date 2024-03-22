using Avalonia;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using OpenGptChat.Views.Pages;

namespace OpenGptChat.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public static readonly StyledProperty<Control?> CurrentPageProperty =
            AvaloniaProperty.Register<MainWindow, Control?>(nameof(CurrentPage), null);

        public Control? CurrentPage
        {
            get => GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value);
        }


        private void GoToConfigPageButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var mainWindow = App.Services.GetRequiredService<MainWindow>();
            mainWindow.CurrentPage = App.Services.GetRequiredService<ConfigPage>();
        }

        private void GoToMainPageButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var mainWindow = App.Services.GetRequiredService<MainWindow>();
            mainWindow.CurrentPage = App.Services.GetRequiredService<MainPage>();
        }
    }
}