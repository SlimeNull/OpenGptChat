using Microsoft.Extensions.Options;
using OpenChat.Models;
using OpenChat.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenChat.ViewModels
{
    public class ConfigPageModel : INotifyPropertyChanged
    {
        public ConfigurationService ConfigurationService { get; }

        public AppConfig Configuration => ConfigurationService.Instance;

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
            foreach (var msg in ConfigurationService.Instance.SystemMessages)
                SystemMessages.Add(new ValueWrapper<string>(msg));
        }

        public void ApplySystemMessages()
        {
            ConfigurationService.Instance.SystemMessages = SystemMessages
                .Select(wrap => wrap.Value)
                .ToArray();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
