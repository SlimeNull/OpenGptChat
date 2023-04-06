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
        private ColorMode currentActualMode =
            SystemHelper.IsDarkTheme() ? ColorMode.Dark : ColorMode.Light;

        public ColorMode CurrentMode
        {
            get => currentMode;
            set
            {
                SwitchTo(value);
            }
        }
        public ColorMode CurrentActualMode => currentActualMode;

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

        public void ApplyThemeForWindow(Window window)
        {
            IntPtr hwnd =
                    new WindowInteropHelper(window).Handle;

            if (hwnd != IntPtr.Zero)
            {
                NativeMethods.EnableDarkModeForWindow(hwnd, CurrentActualMode == ColorMode.Dark);
            }
            else
            {
                EventHandler? handler = null;

                handler = (s, args) =>
                {
                    if (s is Window _window)
                        ApplyThemeForWindow(_window);

                    window.SourceInitialized -= handler;
                };

                window.SourceInitialized += handler;
            }
        }

        private void SwitchToLightModeCore(bool setField)
        {
            SwitchTo(lightMode);

            if (setField)
                ChangeColorModeAndNotify(ColorMode.Light, ColorMode.Light);
        }

        private void SwitchToDarkModeCore(bool setField)
        {
            SwitchTo(darkMode);

            if (setField)
                ChangeColorModeAndNotify(ColorMode.Dark, ColorMode.Dark);
        }

        private void ChangeColorModeAndNotify(ColorMode colorMode, ColorMode actualColorMode)
        {
            if (actualColorMode == ColorMode.Auto)
                throw new ArgumentException($"{nameof(actualColorMode)} cannot be 'Auto'", nameof(actualColorMode));

            bool notify = colorMode != currentMode || actualColorMode != currentActualMode;

            currentMode = colorMode;
            currentActualMode = actualColorMode;

            if (notify)
                ColorModeChanged?.Invoke(this, new ColorModeChangedEventArgs(colorMode, actualColorMode));
        }


        public void SwitchToAuto()
        {
            bool isDarkMode =
                SystemHelper.IsDarkTheme();

            if (isDarkMode)
                SwitchToDarkModeCore(false);
            else
                SwitchToLightModeCore(false);

            ChangeColorModeAndNotify(ColorMode.Auto, isDarkMode ? ColorMode.Dark : ColorMode.Light);

            foreach (Window window in Application.Current.Windows)
                ApplyThemeForWindow(window);
        }

        public void SwitchToLightMode()
        {
            SwitchToLightModeCore(true);

            foreach (Window window in Application.Current.Windows)
                ApplyThemeForWindow(window);
        }

        public void SwitchToDarkMode()
        {
            SwitchToDarkModeCore(true);

            foreach (Window window in Application.Current.Windows)
                ApplyThemeForWindow(window);
        }

        public event EventHandler<ColorModeChangedEventArgs>? ColorModeChanged;
    }

    public class ColorModeChangedEventArgs : EventArgs
    {
        public ColorModeChangedEventArgs(ColorMode colorMode, ColorMode actualColorMode)
        {
            ColorMode = colorMode;
            ActualColorMode = actualColorMode;
        }

        public ColorMode ColorMode { get; }
        public ColorMode ActualColorMode { get; }
    }
}
