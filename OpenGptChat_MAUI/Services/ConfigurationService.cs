using Microsoft.Extensions.Options;
using OpenGptChat_MAUI.Models;
using OpenGptChat_MAUI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenGptChat_MAUI.Services
{
    public class ConfigurationService
    {
        public ConfigurationService(IOptions<AppConfig> configuration)
        {
            OptionalConfiguration = configuration;
        }

        private IOptions<AppConfig> OptionalConfiguration { get; }

        public AppConfig Configuration => OptionalConfiguration.Value;

        public void Save()
        {
            using FileStream fs = File.Create(GlobalValues.JsonConfigurationFilePath);
            JsonSerializer.Serialize(fs, OptionalConfiguration.Value, JsonHelper.ConfigurationOptions);
        }
    }
}
