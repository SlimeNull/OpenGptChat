using CommunityToolkit.Mvvm.ComponentModel;
using OpenGptChat.Models;
using OpenGptChat.Services;

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
