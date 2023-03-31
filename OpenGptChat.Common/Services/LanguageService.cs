using System.Globalization;
using System.Windows;

namespace OpenGptChat.Services
{


    public class LanguageService
    {
        private static string resourceUriPrefix = "pack://application:,,,/OpenGptChat.Common;component";

        private static Dictionary<CultureInfo, ResourceDictionary> languageResources =
            new Dictionary<CultureInfo, ResourceDictionary>()
            {
                { new CultureInfo("en"), new ResourceDictionary() { Source = new Uri($"{resourceUriPrefix}/Languages/en.xaml" ) } },
                { new CultureInfo("zh-hans"), new ResourceDictionary() { Source = new Uri($"{resourceUriPrefix}/Languages/zh-hans.xaml" ) } },
                { new CultureInfo("zh-hant"), new ResourceDictionary() { Source = new Uri($"{resourceUriPrefix}/Languages/zh-hant.xaml" ) } },
                { new CultureInfo("ja"), new ResourceDictionary() { Source = new Uri($"{resourceUriPrefix}/Languages/ja.xaml" ) } },
                { new CultureInfo("ar"), new ResourceDictionary() { Source = new Uri($"{resourceUriPrefix}/Languages/ar.xaml" ) } },
                { new CultureInfo("es"), new ResourceDictionary() { Source = new Uri($"{resourceUriPrefix}/Languages/es.xaml" ) } },
                { new CultureInfo("fr"), new ResourceDictionary() { Source = new Uri($"{resourceUriPrefix}/Languages/fr.xaml" ) } },
                { new CultureInfo("ru"), new ResourceDictionary() { Source = new Uri($"{resourceUriPrefix}/Languages/ru.xaml" ) } },
                { new CultureInfo("ur"), new ResourceDictionary() { Source = new Uri($"{resourceUriPrefix}/Languages/ur.xaml" ) } },
            };

        private static CultureInfo defaultLanguage =
            new CultureInfo("en");

        public LanguageService(
            ConfigurationService configurationService)
        {
            // 如果配置文件里面有置顶语言, 则设置语言
            CultureInfo language = CultureInfo.CurrentCulture;
            if (!string.IsNullOrWhiteSpace(configurationService.Configuration.Language))
                language = new CultureInfo(configurationService.Configuration.Language);

            SetLanguage(language);
        }




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
