using Microsoft.Extensions.Options;
using OpenGptChat.Models;
using OpenGptChat.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGptChat.ViewModels
{
    public class ConfigPageModel : INotifyPropertyChanged
    {
        public ConfigurationService ConfigurationService { get; }

        public AppConfig Configuration => ConfigurationService.Configuration;

        public ConfigPageModel(ConfigurationService configurationService)
        {
            ConfigurationService = configurationService;

            LoadSystemMessages();
        }

        public ObservableCollection<ValueWrapper<string>> SystemMessages { get; }
            = new ObservableCollection<ValueWrapper<string>>();

        public void LoadSystemMessages()
        {
            SystemMessages.Clear();
            foreach (var msg in ConfigurationService.Configuration.SystemMessages)
                SystemMessages.Add(new ValueWrapper<string>(msg));
        }

        public void ApplySystemMessages()
        {
            ConfigurationService.Configuration.SystemMessages = SystemMessages
                .Select(wrap => wrap.Value)
                .ToArray();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
