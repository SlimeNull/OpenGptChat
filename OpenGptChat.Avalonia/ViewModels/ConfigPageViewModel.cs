using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using OpenGptChat.Services;

namespace OpenGptChat.ViewModels
{
    public partial class ConfigPageViewModel : ObservableObject
    {
        public ConfigService ConfigService { get; set; }
            = App.Services.GetRequiredService<ConfigService>();
    }
}
