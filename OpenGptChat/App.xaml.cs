using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenGptChat.Models;
using OpenGptChat.Services;
using OpenGptChat.Utilities;
using OpenGptChat.ViewModels;
using OpenGptChat.Views;
using OpenGptChat.Views.Dialogs;
using OpenGptChat.Views.Pages;

namespace OpenGptChat
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private static readonly IHost host = Host
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
                services.AddSingleton<PageService>();
                services.AddSingleton<NoteService>();
                services.AddSingleton<ChatService>();
                services.AddSingleton<ChatPageService>();
                services.AddSingleton<ChatStorageService>();
                services.AddSingleton<ConfigurationService>();
                services.AddSingleton<SmoothScrollingService>();

                // 适应
                services.AddSingleton<LanguageService>();
                services.AddSingleton<ColorModeService>();
                
                services.AddSingleton<AppWindow>();
                services.AddSingleton<MainPage>();
                services.AddSingleton<ConfigPage>();

                services.AddSingleton<AppWindowModel>();
                services.AddSingleton<MainPageModel>();
                services.AddSingleton<ConfigPageModel>();

                // 作用域服务
                services.AddScoped<ChatPage>();
                services.AddScoped<ChatPageModel>();

                // 配置服务, 将配置与 AppConfig 绑定
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
            // 确认程序是单例?
            if (!EnsureAppSingletion())
            {
                Application.Current.Shutdown();
                return;
            }

            await host.StartAsync();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await host.StopAsync();

            host.Dispose();
        }


        public static IRelayCommand ShowAppCommand =
            new RelayCommand(ShowApp);
        public static IRelayCommand HideAppCommand =
            new RelayCommand(HideApp);
        public static IRelayCommand CloseAppCommand =
            new RelayCommand(CloseApp);

        public static void ShowApp()
        {
            Window mainWindow = Application.Current.MainWindow;
            if (mainWindow == null)
                return;

            mainWindow.Show();

            if (mainWindow.WindowState == WindowState.Minimized)
                mainWindow.WindowState = WindowState.Normal;

            if (!mainWindow.IsActive)
                mainWindow.Activate();
        }

        public static void HideApp()
        {
            Window mainWindow = Application.Current.MainWindow;
            if (mainWindow == null)
                return;

            mainWindow.Hide();
        }

        public static void CloseApp()
        {
            Application.Current.Shutdown();
        }


        /// <summary>
        /// 确认程序是单例运行的 / Confirm that the program is running as a singleton.
        /// </summary>
        /// <returns>当前程序是否是单例, 如果 false, 那么应该立即中止程序</returns>
        public bool EnsureAppSingletion()
        {
            EventWaitHandle singletonEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "SlimeNull/OpenGptChat", out bool createdNew);

            if (createdNew)
            {
                Task.Run(() =>
                {
                    while (true)
                    {
                        // wait for the second instance of OpenGptChat
                        singletonEvent.WaitOne();

                        Dispatcher.Invoke(() =>
                        {
                            ShowApp();
                        });
                    }
                });

                return true;
            }
            else
            {
                singletonEvent.Set();
                return false;
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"{e.ExceptionObject}", "UnhandledException", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
