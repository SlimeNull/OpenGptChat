using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OpenGptChat.Services
{
    public class LanguageService
    {
        private static Dictionary<CultureInfo, ResourceDictionary> languageResources =
            new Dictionary<CultureInfo, ResourceDictionary>()
            {
                { new CultureInfo("en"), new ResourceDictionary() { Source = new Uri("/Languages/en.xaml", UriKind.Relative) } },
                { new CultureInfo("zh-hans"), new ResourceDictionary() { Source = new Uri("/Languages/zh-hans.xaml", UriKind.Relative) } },
                { new CultureInfo("zh-hant"), new ResourceDictionary() { Source = new Uri("/Languages/zh-hant.xaml", UriKind.Relative) } },
                { new CultureInfo("ja-JP"), new ResourceDictionary() { Source = new Uri("/Languages/ja-JP.xaml", UriKind.Relative) } },
                { new CultureInfo("ar"), new ResourceDictionary() { Source = new Uri("/Languages/ar.xaml", UriKind.Relative) } },
                { new CultureInfo("es"), new ResourceDictionary() { Source = new Uri("/Languages/es.xaml", UriKind.Relative) } },
                { new CultureInfo("fr"), new ResourceDictionary() { Source = new Uri("/Languages/fr.xaml", UriKind.Relative) } },
                { new CultureInfo("ru"), new ResourceDictionary() { Source = new Uri("/Languages/ru.xaml", UriKind.Relative) } },
                { new CultureInfo("ur"), new ResourceDictionary() { Source = new Uri("/Languages/ur.xaml", UriKind.Relative) } },
            };

        private static CultureInfo defaultLanguage =
            new CultureInfo("en");

        private CultureInfo currentLanguage =
            defaultLanguage;

        public IEnumerable<CultureInfo> Languages =>
            languageResources.Keys;

        public CultureInfo CurrentLanguage
        {
            get => currentLanguage;
            set
            {
                if (!SetLanguage(value))
                    throw new ArgumentException("Unsupport language");
            }
        }
        public bool SetLanguage(CultureInfo language)
        {
            // 查找一个合适的 key
            CultureInfo? key = Languages
                .Where(key => key.Equals(language))
                .FirstOrDefault();

            if (key == null)
                key = Languages
                    .Where(key => key.TwoLetterISOLanguageName == language.TwoLetterISOLanguageName)
                    .FirstOrDefault();

            if (key != null)
            {
                ResourceDictionary? resourceDictionary = languageResources[key];

                var oldLanguageResources =
                    Application.Current.Resources.MergedDictionaries
                        .Where(dict => dict.Contains("IsLanguageResource"))
                        .ToList();

                foreach (var res in oldLanguageResources)
                    Application.Current.Resources.MergedDictionaries.Remove(res);

                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);

                currentLanguage = key;
                return true;
            }

            return false;
        }
    }
}
