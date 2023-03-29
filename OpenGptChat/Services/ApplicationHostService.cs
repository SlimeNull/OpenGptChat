using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using OpenGptChat.Models;
using OpenGptChat.Utilities;
using OpenGptChat.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace OpenGptChat.Services
{
    internal class ApplicationHostService : IHostedService
    {
        public ApplicationHostService(
            LanguageService languageService,
            ConfigurationService configurationService)
        {
            LanguageService = languageService;
            ConfigurationService = configurationService;
        }

        public LanguageService LanguageService { get; }
        public ConfigurationService ConfigurationService { get; }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (!File.Exists(GlobalValues.JsonConfigurationFilePath))
                ConfigurationService.Save();

            CultureInfo language = CultureInfo.CurrentCulture;
            if (!string.IsNullOrWhiteSpace(ConfigurationService.Configuration.Language))
                language = new CultureInfo(ConfigurationService.Configuration.Language);

            LanguageService.SetLanguage(language);

            if (!App.Current.Windows.OfType<AppWindow>().Any())
            {
                AppWindow window = App.GetService<AppWindow>();
                window.Show();

                window.Navigate(App.GetService<MainPage>());
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
