using Microsoft.Extensions.Options;
using OpenChat.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenChat.ViewModels
{
    public class ConfigPageModel : INotifyPropertyChanged
    {
        public ConfigPageModel(IOptions<AppConfig> config)
        {
            Config = config;
        }

        public IOptions<AppConfig> Config { get; }
        public AppConfig ConfigInstance => Config.Value;

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
