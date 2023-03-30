using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGptChat_MAUI.Utilities
{
    internal class GlobalValues
    {
        public static string AppName => "OpenGptChat";

        public static string DatabaseFilename = "chat.db";

        public static string JsonConfigurationFileName = "AppConfig.json";

        public static string JsonConfigurationFilePath=>
            Path.Combine(FileSystem.AppDataDirectory, JsonConfigurationFileName);
            


        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
    }
}
