using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.Styling;

namespace OpenGptChat.Strings
{
    public class StringResources : ResourceDictionary
    {
        private static readonly string s_assemblyName = Assembly.GetExecutingAssembly().GetName().Name ?? string.Empty;

        private static readonly Dictionary<CultureInfo, IResourceProvider> _resourceDictionaries = new()
        {
            [new CultureInfo("en-us")] = StringsResourceProvider("Base.axaml"),
            [new CultureInfo("zh-Hans")] = StringsResourceProvider("ChineseSimplified.axaml"),
            [new CultureInfo("zh-Hant")] = StringsResourceProvider("ChineseTraditional.axaml"),
            [new CultureInfo("ar")] = StringsResourceProvider("Arabic.axaml"),
            [new CultureInfo("es")] = StringsResourceProvider("Spanish.axaml"),
            [new CultureInfo("fr")] = StringsResourceProvider("French.axaml"),
            [new CultureInfo("ja")] = StringsResourceProvider("Japanese.axaml"),
            [new CultureInfo("ru")] = StringsResourceProvider("Rusian.axaml"),
            [new CultureInfo("tr")] = StringsResourceProvider("Turkish.axaml"),
            [new CultureInfo("ur")] = StringsResourceProvider("Urdu.axaml"),
        };

        private static ResourceInclude StringsResourceProvider(string fileName)
        {
            return new(new Uri($"avares://{s_assemblyName}"))
            {
                Source = new Uri($"/Strings/{fileName}", UriKind.Relative)
            };
        }

        private static CultureInfo FindBestMatchCultureInfo(IEnumerable<CultureInfo> cultureInfos, CultureInfo toFind)
        {
            var matched =
                cultureInfos.FirstOrDefault(cultureInfo => cultureInfo == toFind) ??
                cultureInfos.FirstOrDefault(cultureInfo => cultureInfo?.TwoLetterISOLanguageName == toFind?.TwoLetterISOLanguageName) ??
                cultureInfos.FirstOrDefault();

            if (matched == null)
                throw new ArgumentException($"No item in '{nameof(cultureInfos)}'", nameof(cultureInfos));

            return matched;
        }


        private CultureInfo _culture;

        private StringResources()
        {
            _culture = CultureInfo.CurrentCulture;
            OnCultureChanged();
        }

        public static StringResources Instance { get; } = new();

        private void OnCultureChanged()
        {
            var key = FindBestMatchCultureInfo(_resourceDictionaries.Keys, _culture);
            var resourceProvider = _resourceDictionaries[key];

            MergedDictionaries.Clear();
            MergedDictionaries.Add(resourceProvider);

            CultureChanged?.Invoke(this, EventArgs.Empty);
        }

        public CultureInfo Culture 
        { 
            get => _culture; 
            set
            {
                _culture = value;
                OnCultureChanged();
            }
        }

        public event EventHandler? CultureChanged;
    }
}
