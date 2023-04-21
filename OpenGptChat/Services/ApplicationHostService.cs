using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenGptChat.Markdown;
using OpenGptChat.Utilities;
using OpenGptChat.Views;
using OpenGptChat.Views.Pages;

namespace OpenGptChat.Services
{
    public class ApplicationHostService : IHostedService
    {
        public ApplicationHostService(
            IServiceProvider serviceProvider,
            ChatStorageService chatStorageService,
            ConfigurationService configurationService)
        {
            ServiceProvider = serviceProvider;
            ChatStorageService = chatStorageService;
            ConfigurationService = configurationService;
        }

        public IServiceProvider ServiceProvider { get; }
        public ChatStorageService ChatStorageService { get; }
        public ConfigurationService ConfigurationService { get; }



        public Task StartAsync(CancellationToken cancellationToken)
        {
            // 如果不存在配置文件则保存一波
            if (!File.Exists(GlobalValues.JsonConfigurationFilePath))
                ConfigurationService.Save();

            // 初始化服务
            ChatStorageService.Initialize();

            // 初始化 Markdown 渲染
            MarkdownWpfRenderer.LinkNavigate += (s, e) =>
            {
                try
                {
                    if (e.Link != null)
                        Process.Start("Explorer.exe", new string[] { e.Link });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Cannot open link: {ex}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };

            // 启动主窗体
            if (!Application.Current.Windows.OfType<AppWindow>().Any())
            {
                AppWindow window = ServiceProvider.GetService<AppWindow>() ?? throw new InvalidOperationException("Cannot find MainWindow service");
                window.Show();

                // 导航到主页
                window.Navigate<MainPage>();
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // 关闭存储服务
            ChatStorageService.Dispose();

            return Task.CompletedTask;
        }
    }
}
