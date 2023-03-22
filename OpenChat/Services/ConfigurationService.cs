using Microsoft.Extensions.Options;
using OpenChat.Models;
using OpenChat.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenChat.Services
{
    public class ConfigurationService
    {
        public ConfigurationService(IOptions<AppConfig> configuration)
        {
            Configuration = configuration;
        }

        private IOptions<AppConfig> Configuration { get; }

        public AppConfig Instance => Configuration.Value;

        public void Save()
        {
            using FileStream fs = File.OpenWrite(Strings.JsonConfigurationFilePath);
            JsonSerializer.Serialize(fs, Configuration.Value, JsonHelper.ConfigurationOptions);
        }
    }
}
