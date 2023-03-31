using System.Diagnostics;
using System.IO;

namespace OpenGptChat.Utilities
{
    public class GlobalValues
    {
        public static string AppName => nameof(OpenGptChat);

        public static string JsonConfigurationFilePath { get; } =
            Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName) ?? "./", "AppConfig.json");
    }
}
