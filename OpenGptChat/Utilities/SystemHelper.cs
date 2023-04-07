using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

namespace OpenGptChat.Utilities
{
    public static class SystemHelper
    {
        private static bool IsDarkThemeDependOnColor()
        {
            var themeColor = SystemParameters.WindowGlassColor;
            var brightness = ColorHelper.GetBrightness(themeColor.R, themeColor.G, themeColor.B);

            return brightness < 0.3;
        }

        private static bool? IsDarkThemeDependOnRegistry()
        {
            const string RegistryKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
            const string RegistryValueName = "AppsUseLightTheme";
            // 这里也可能是LocalMachine(HKEY_LOCAL_MACHINE)
            // see "https://www.addictivetips.com/windows-tips/how-to-enable-the-dark-theme-in-windows-10/"
            object? registryValueObject = Registry.CurrentUser.OpenSubKey(RegistryKeyPath)?.GetValue(RegistryValueName);
            if (registryValueObject is null)
                return null;
            return (int)registryValueObject > 0 ? false : true;
        }

        public static bool IsDarkTheme()
        {
            return IsDarkThemeDependOnRegistry() ?? IsDarkThemeDependOnColor();
        }
    }
}
