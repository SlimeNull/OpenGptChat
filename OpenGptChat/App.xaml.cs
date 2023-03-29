using CommunityToolkit.Mvvm.Input;
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
using System.Diagnostics;
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
                services.AddSingleton<LanguageService>();

                // 窗体服务
                services.AddSingleton<AppWindow>();
                services.AddSingleton<AppWindowModel>();

                // 单例页面服务
                services.AddSingleton<MainPage>();
                services.AddSingleton<MainPageModel>();
                services.AddSingleton<ConfigPage>();
                services.AddSingleton<ConfigPageModel>();

                // 作用域页面服务
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


        /// <summary>
        /// 确认程序是单例运行的
        /// </summary>
        /// <returns></returns>
        public bool EnsureAppSingletion()
        {
            // 拿到当前线程
            Process currentProcess = Process.GetCurrentProcess();

            // 找与当前线程同名的线程
            Process[] processes =
                Process.GetProcessesByName(currentProcess.ProcessName);

            // 循环
            foreach (Process process in processes)
            {
                // 如果线程与当前线程 ID 一样, 跳过
                if (process.Id == currentProcess.Id)
                    continue;

                // 取主窗口并置顶, 成功了就返回 false (表示当前程序不是单例的)
                IntPtr mainWindowHandle = process.MainWindowHandle;
                if (mainWindowHandle != IntPtr.Zero &&
                    NativeMethods.ShowWindowNormal(mainWindowHandle) &&
                    NativeMethods.SetForegroundWindow(mainWindowHandle))
                    return false;
            }

            // 这说明没有找到合适的进程, 返回 true (表示当前程序是第一个启动的实例)
            return true;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"{e.ExceptionObject}", "UnhandledException", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
