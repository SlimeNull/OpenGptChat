using System.Windows;
using OpenGptChat.Common.Models;
using OpenGptChat.Common.Utilities;

namespace OpenGptChat.Services
{
    public class ColorModeService
    {
        private static string resourceUriPrefix = "pack://application:,,,/OpenGptChat.Common;component";

        private ResourceDictionary brightMode =
            new ResourceDictionary() { Source = new Uri($"{resourceUriPrefix}/ColorModes/BrightMode.xaml") };

        private ResourceDictionary darkMode =
            new ResourceDictionary() { Source = new Uri($"{resourceUriPrefix}/ColorModes/DarkMode.xaml") };

        public ColorModeService(
            ConfigurationService configurationService)
        {
            var configurationColorMode = 
                configurationService.Configuration.ColorMode;

            SwitchTo(configurationColorMode);
        }


        private ColorMode currentMode = ColorMode.Auto;

        public ColorMode CurrentMode
        {
            get => currentMode;
            set
            {
                SwitchTo(value);
            }
        }

        public IEnumerable<ColorMode> ColorModes =>
            Enum.GetValues<ColorMode>();

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
                case ColorMode.Bright:
                    SwitchToBright();
                    break;
                case ColorMode.Dark:
                    SwitchToDark();
                    break;
                case ColorMode.Auto:
                    SwitchToAuto();
                    break;
                default:
                    throw new ArgumentException("Must be bright or dark");
            }
        }

        public void SwitchToAuto()
        {
            if (SystemHelper.IsDarkTheme())
                SwitchTo(darkMode);
            else
                SwitchTo(brightMode);

            currentMode = ColorMode.Auto;
        }

        public void SwitchToBright()
        {
            SwitchTo(brightMode);
            currentMode = ColorMode.Bright;
        }

        public void SwitchToDark()
        {
            SwitchTo(darkMode);
            currentMode = ColorMode.Dark;
        }
    }
}
