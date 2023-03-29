using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGptChat.Models
{
    public partial class AppConfig : ObservableObject
    {
        [ObservableProperty]
        private string _apiHost = "openaiapi.elecho.top";

        [ObservableProperty]
        private string _apiKey = string.Empty;

        [ObservableProperty]
        private string _apiGptModel = "gpt-3.5-turbo";

        [ObservableProperty]
        private int _apiTimeout = 5000;

        [ObservableProperty]
        private double _temerature = 1;

        [ObservableProperty]
        private string[] _systemMessages = new string[]
        {

        };

        [ObservableProperty]
        private string _language = string.Empty;

        [ObservableProperty]
        private bool _windowAlwaysOnTop = false;

        [ObservableProperty]
        private string _chatStoragePath = "chat.db";
    }
}
