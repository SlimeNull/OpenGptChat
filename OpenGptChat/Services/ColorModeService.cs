using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using Microsoft.Win32;
using OpenGptChat.Common.Models;
using OpenGptChat.Common.Utilities;

namespace OpenGptChat.Services
{
    public class ColorModeService
    {
        private static string resourceUriPrefix = "pack://application:,,,";

        private ResourceDictionary lightMode =
            new ResourceDictionary() { Source = new Uri($"{resourceUriPrefix}/ColorModes/LightMode.xaml") };

        private ResourceDictionary darkMode =
            new ResourceDictionary() { Source = new Uri($"{resourceUriPrefix}/ColorModes/DarkMode.xaml") };

        public ColorModeService(
            ConfigurationService configurationService)
        {
            ConfigurationService = configurationService;
        }

        private ConfigurationService ConfigurationService { get; }

        private void InitMessageHook()
        {
            SystemEvents.UserPreferenceChanged += 
                SystemEvents_UserPreferenceChanged;
        }

        private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category == UserPreferenceCategory.General)
                SwitchTo(CurrentMode);
        }

        public void Init()
        {
            var configurationColorMode =
                ConfigurationService.Configuration.ColorMode;

            SwitchTo(configurationColorMode);
            InitMessageHook();
        }

        public IEnumerable<ColorMode> ColorModes =>
            Enum.GetValues<ColorMode>();

        private ColorMode currentMode = 
            ColorMode.Auto;
        public ColorMode CurrentMode
        {
            get => currentMode;
            set
            {
                SwitchTo(value);
            }
        }

        private void SwitchTo(ResourceDictionary colorModeResource)
        {
            var oldColorModeResources =
                Application.Current.Resources.MergedDictionaries
                    .Where(dict => dict.Contains("IsColorModeResource"))
                    .ToList();

            foreach (var res in oldColorModeResources)
                Application.Current.Resources.MergedDictionaries.Remove(res);

            Application.Current.Resources.MergedDictionaries.Add(colorModeResource);
        }

        public void SwitchTo(ColorMode mode)
        {
            switch (mode)
            {
                case ColorMode.Light:
                    SwitchToLightMode();
                    break;
                case ColorMode.Dark:
                    SwitchToDarkMode();
                    break;
                case ColorMode.Auto:
                    SwitchToAuto();
                    break;
                default:
                    throw new ArgumentException("Must be bright or dark");
            }
        }

        private void SwitchToLightModeCore(bool setField)
        {
            SwitchTo(lightMode);

            if (setField)
                currentMode = ColorMode.Light;

            if (Application.Current.MainWindow is Window window)
            {
                IntPtr hwnd =
                    new WindowInteropHelper(window).Handle;

                NativeMethods.EnableDarkModeForWindow(hwnd, false);
            }
        }

        private void SwitchToDarkModeCore(bool setField)
        {
            SwitchTo(darkMode);

            if (setField)
                currentMode = ColorMode.Dark;

            if (Application.Current.MainWindow is Window window)
            {
                IntPtr hwnd =
                    new WindowInteropHelper(window).Handle;

                NativeMethods.EnableDarkModeForWindow(hwnd, true);
            }
        }


        public void SwitchToAuto()
        {
            if (SystemHelper.IsDarkTheme())
                SwitchToDarkModeCore(false);
            else
                SwitchToLightModeCore(false);

            currentMode = ColorMode.Auto;
        }

        public void SwitchToLightMode()
        {
            SwitchToLightModeCore(true);
        }

        public void SwitchToDarkMode()
        {
            SwitchToDarkModeCore(true);
        }
    }
}
