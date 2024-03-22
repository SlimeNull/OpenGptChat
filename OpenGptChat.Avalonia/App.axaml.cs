using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using OpenGptChat.Services;
using OpenGptChat.Strings;
using OpenGptChat.ViewModels;
using OpenGptChat.Views;
using OpenGptChat.Views.Pages;

namespace OpenGptChat
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; }
            = BuildServiceProvider();

        private static IServiceProvider BuildServiceProvider()
        {
            return new ServiceCollection()
                .AddSingleton<ChatService>()
                .AddSingleton<ConfigService>()
                .AddSingleton<StorageService>()
                .AddSingleton<TitleGenerationService>()
                .AddSingleton<MainWindow>()
                .AddSingleton<MainPage>()
                .AddSingleton<ConfigPage>()
                .BuildServiceProvider();
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);

            if (Resources is null)
                Resources = new ResourceDictionary();

            Resources.MergedDictionaries.Add(
                StringResources.Instance);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Line below is needed to remove Avalonia data validation.
                // Without this line you will get duplicate validations from both Avalonia and CT

                var mainWindow = Services.GetRequiredService<MainWindow>();
                var mainPage = Services.GetRequiredService<MainPage>();

                mainWindow.CurrentPage = mainPage;

                BindingPlugins.DataValidators.RemoveAt(0);
                desktop.MainWindow = mainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}