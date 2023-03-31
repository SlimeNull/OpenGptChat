using System.IO;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenGptChat.Abstraction;
using OpenGptChat.Utilities;

namespace OpenGptChat.Services
{
    public class ApplicationHostService : IHostedService
    {
        public ApplicationHostService(
            IServiceProvider serviceProvider,
            ChatStorageService chatStorageService,
            ConfigurationService configurationService,
            LanguageService languageService,     // just for initialize these services
            ColorModeService colorModeService)
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

            // 启动主窗体
            if (!Application.Current.Windows.OfType<IAppWindow>().Any())
            {
                IAppWindow window = ServiceProvider.GetService<IAppWindow>() ?? throw new InvalidOperationException("Cannot find MainWindow service");
                window.Show();

                // 导航到主页
                window.Navigate<IMainPage>();
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
