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
        public ConfigurationService Configuration { get; }

        public AppConfig ConfigurationInstance => Configuration.Instance;

        public ConfigPageModel(ConfigurationService configuration)
        {
            Configuration = configuration;

            LoadSystemMessages();
        }

        public ObservableCollection<ValueWrapper<string>> SystemMessages { get; }
            = new ObservableCollection<ValueWrapper<string>>();

        public void LoadSystemMessages()
        {
            SystemMessages.Clear();
            foreach (var msg in Configuration.Instance.SystemMessages)
                SystemMessages.Add(new ValueWrapper<string>(msg));
        }

        public void ApplySystemMessages()
        {
            Configuration.Instance.SystemMessages = SystemMessages
                .Select(wrap => wrap.Value)
                .ToArray();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
