using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using OpenGptChat.Utilities;

namespace OpenGptChat.Services
{
    public class ConfigService
    {
        public string FilePath { get; set; } = "AppConfig.json";
        public AppConfig AppConfig { get; }

        public ConfigService()
        {
            // initialize configuration

            string fullFilePath = Path.Combine(
                Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName) ?? "./", FilePath);

            if (File.Exists(fullFilePath))
            {
                string json = File.ReadAllText(fullFilePath);
                var config = JsonSerializer.Deserialize<AppConfig>(json, JsonHelper.ConfigurationOptions);

                if (config != null)
                    AppConfig = config;
            }

            if (AppConfig == null)
                AppConfig = new();
        }
    }
}
