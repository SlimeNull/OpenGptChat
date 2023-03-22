using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using OpenChat.Models;
using OpenChat.Utilities;
using OpenChat.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OpenChat.Services
{
    internal class ApplicationHostService : IHostedService
    {
        public ApplicationHostService(ConfigurationService config)
        {
            Configuration = config;
        }

        public ConfigurationService Configuration { get; }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (!File.Exists(Strings.JsonConfigurationFilePath))
                Configuration.Save();

            if (!App.Current.Windows.OfType<MainWindow>().Any())
            {
                MainWindow window = App.GetService<MainWindow>();
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
