using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenChat.Utilities
{
    internal class Strings
    {
        public static string JsonConfigurationFilePath { get; } =
            Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!, "AppConfig.json");
    }
}
