using OpenChat.Models;
using OpenChat.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenChat.ViewModels
{
    public class AppWindowModel : INotifyPropertyChanged
    {
        public AppWindowModel(ConfigurationService configurationService)
        {
            ConfigurationService = configurationService;
        }

        public ConfigurationService ConfigurationService { get; }

        public AppConfig Configuration => ConfigurationService.Instance;

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
