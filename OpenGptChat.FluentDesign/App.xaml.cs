using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using OpenGptChat.Abstraction;
using OpenGptChat.FluentDesign.Services;
using OpenGptChat.FluentDesign.Views.Pages;
using OpenGptChat.FluentDesign.Views.Windows;
using OpenGptChat.Models;
using OpenGptChat.Services;
using OpenGptChat.Utilities;
using OpenGptChat.ViewModels;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;

namespace OpenGptChat.FluentDesign
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        static App()
        {
            GlobalServices.InitServices(_host.Services);
        }

        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(config =>
            {
                // 支持使用 JSON 文件以及环境变量进行配置
                config
                    .AddJsonFile(GlobalValues.JsonConfigurationFilePath, true, true)
                    .AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                // 程序托管服务
                services.AddHostedService<ApplicationHostService>();

                // 添加基础服务
                services.AddSingleton<NoteService>();
                services.AddSingleton<ChatService>();
                services.AddSingleton<ChatPageService>();
                services.AddSingleton<ChatStorageService>();
                services.AddSingleton<ConfigurationService>();
                services.AddSingleton<SmoothScrollingService>();
                services.AddSingleton<LanguageService>();

                // 替换掉的基础服务
                services.AddSingleton<IPageService, FluentPageService>();

                // 页面
                services.AddSingleton<AppWindow>();
                services.AddSingleton<MainPage>();
                services.AddSingleton<ConfigPage>();
                services.AddSingleton<IAppWindow>(services => services.GetService<AppWindow>()!);
                services.AddSingleton<IMainPage>(services => services.GetService<MainPage>()!);
                services.AddSingleton<IConfigPage>(services => services.GetService<ConfigPage>()!);

                services.AddSingleton<AppWindowModel>();
                services.AddSingleton<MainPageModel>();
                services.AddSingleton<ConfigPageModel>();

                // 作用域页面服务
                services.AddScoped<ChatPage>();
                services.AddScoped<IChatPage>(services => services.GetService<ChatPage>()!);

                services.AddScoped<ChatPageModel>();

                // 配置服务, 将配置与 AppConfig 绑定
                services.Configure<AppConfig>(
                    o =>
                    {
                        context.Configuration.Bind(o);
                    });
            })
            .Build();

        /// <summary>
        /// Gets registered service.
        /// </summary>
        /// <typeparam name="T">Type of the service to get.</typeparam>
        /// <returns>Instance of the service or <see langword="null"/>.</returns>
        public static T GetService<T>()
            where T : class
        {
            return _host.Services.GetService<T>() ?? throw new ArgumentException("Cannot find service with specified type");
        }

        /// <summary>
        /// Occurs when the application is loading.
        /// </summary>
        private async void OnStartup(object sender, StartupEventArgs e)
        {
            await _host.StartAsync();
        }

        /// <summary>
        /// Occurs when the application is closing.
        /// </summary>
        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();

            _host.Dispose();
        }

        /// <summary>
        /// Occurs when an exception is thrown by an application but not handled.
        /// </summary>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
        }
    }
}