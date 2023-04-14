using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenGptChat.Utilities
{
    internal static class FileSystemUtils
    {
        public static string GetEntryPointFolder()
        {
            Assembly? entryPointAsm =
                Assembly.GetEntryAssembly();

            if (entryPointAsm == null)
                throw new InvalidOperationException("App was started from unmanaged code");

            return Path.GetDirectoryName(
                entryPointAsm.Location) ?? ".";
        }
    }
}
