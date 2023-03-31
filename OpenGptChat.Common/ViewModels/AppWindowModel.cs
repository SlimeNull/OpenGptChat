using CommunityToolkit.Mvvm.ComponentModel;
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
    public class AppWindowModel : ObservableObject
    {
        public AppWindowModel(ConfigurationService configurationService)
        {
            ConfigurationService = configurationService;
        }

        public string ApplicationTitle { get; } = nameof(OpenGptChat);

        public ConfigurationService ConfigurationService { get; }

        public AppConfig Configuration => ConfigurationService.Configuration;
    }
}
