using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Microsoft.Extensions.DependencyInjection;
using OpenGptChat.Strings;
using OpenGptChat.Views.Pages;

namespace OpenGptChat.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CurrentPageProperty.Changed.Subscribe(new CurrentPagePropertyObserver());
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

        class CurrentPagePropertyObserver : IObserver<AvaloniaPropertyChangedEventArgs<Control?>>
        {
            public void OnCompleted() { }
            public void OnError(Exception error) { }
            public void OnNext(AvaloniaPropertyChangedEventArgs<Control?> e)
            {
                if (e.Sender is not MainWindow mainWindow)
                    return;

                if (e.NewValue.HasValue)
                {
                    var newValue = e.NewValue.Value;
                    mainWindow.MainPageButtons.IsVisible = newValue is MainPage;
                    mainWindow.ConfigPageButtons.IsVisible = newValue is ConfigPage;

                    if (newValue is not null && 
                        newValue.Tag is not null)
                    {
                        mainWindow.PageTitle.Bind(TextBlock.TextProperty, new DynamicResourceExtension(newValue.Tag));
                    }
                }
                else
                {
                    mainWindow.MainPageButtons.IsVisible = false;
                    mainWindow.ConfigPageButtons.IsVisible = false;
                }
            }
        }
    }
}