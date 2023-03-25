using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using OpenGptChat.Models;
using OpenGptChat.Services;
using OpenGptChat.Utilities;
using OpenGptChat.ViewModels;
using OpenGptChat.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace OpenGptChat
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly IHost host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(config =>
            {
                config
                    .AddJsonFile(GlobalValues.JsonConfigurationFilePath, true, true)
                    .AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                services.AddHostedService<ApplicationHostService>();

                services.AddSingleton<PageService>();
                services.AddSingleton<ChatService>();
                services.AddSingleton<NoteService>();
                services.AddSingleton<ConfigurationService>();
                services.AddSingleton<SmoothScrollingService>();

                services.AddSingleton<AppWindow>();
                services.AddSingleton<AppWindowModel>();

                services.AddSingleton<MainPage>();
                services.AddSingleton<MainPageModel>();
                services.AddSingleton<ConfigPage>();
                services.AddSingleton<ConfigPageModel>();

                services.AddSingleton<JsonSerializerOptions>(
                    services => new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = SnakeCaseNamingPolicy.SnakeCase,
                    });

                services.Configure<AppConfig>(
                    o =>
                    {
                        context.Configuration.Bind(o);
                    });
            })
            .Build();

        public static T GetService<T>()
            where T : class
        {
            return (host.Services.GetService(typeof(T)) as T) ?? throw new Exception("Cannot find service of specified type");
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await host.StartAsync();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await host.StopAsync();

            host.Dispose();
        }
    }
}
