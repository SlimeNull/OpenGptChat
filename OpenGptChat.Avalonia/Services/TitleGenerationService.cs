using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using OpenGptChat.Strings;

namespace OpenGptChat.Services
{
    public class TitleGenerationService
    {
        HttpClient httpClient = new HttpClient();

        public async Task<string?> GenerateAsync(string[] messages)
        {
            string languageCode = StringResources.Instance.Culture.Name;

            if (languageCode == "zh-Hans")
                languageCode = "zh-CN";      // 伞兵 Edge API 只能识别 zh-CN, 不能识别 zh-Hans

            object payload = new
            {
                experimentId = string.Empty,
                language = languageCode,
                targetGroup = messages
                    .Select(msg => new
                    {
                        title = msg,
                        url = "https://question.com"
                    })
                    .ToArray()
            };

            var response = await httpClient.PostAsJsonAsync(
                "https://edge.microsoft.com/taggrouptitlegeneration/api/TitleGeneration/gen", payload);

            if (!response.IsSuccessStatusCode)
                return null;

            try
            {
                Dictionary<string, double>? titles = await response.Content.ReadFromJsonAsync<Dictionary<string, double>>();

                if (titles == null || titles.Count == 0)
                    return null;

                return titles.MaxBy(title => title.Value).Key;
            }
            catch
            {
                return null;
            }
        }
    }
}
