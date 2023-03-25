using OpenGptChat.Models;
using OpenGptChat.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGptChat.ViewModels
{
    public class AppWindowModel : INotifyPropertyChanged
    {
        public AppWindowModel(ConfigurationService configurationService)
        {
            ConfigurationService = configurationService;
        }

        public ConfigurationService ConfigurationService { get; }

        public AppConfig Configuration => ConfigurationService.Configuration;

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
