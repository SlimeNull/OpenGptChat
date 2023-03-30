using OpenGptChat_MAUI.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OpenGptChat_MAUI.Services
{
    public class InitService:IMauiInitializeService
    {

        public ChatStorageService ChatStorageService { get; private set; }
        public ConfigurationService ConfigurationService { get; private set; }
        public void Initialize(IServiceProvider services)
        {
            Debug.WriteLine("Start Initiablze !");
            ChatStorageService = services.GetRequiredService<ChatStorageService>();
            ConfigurationService = services.GetRequiredService<ConfigurationService>();



            Debug.WriteLine(GlobalValues.JsonConfigurationFilePath);
            Debug.WriteLine(GlobalValues.DatabasePath);


            // 如果不存在配置文件则保存一波
            if (!File.Exists(GlobalValues.JsonConfigurationFilePath))
                ConfigurationService.Save();

            // 初始化服务
            ChatStorageService.Initialize();



        }
    }
}
