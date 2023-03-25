using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenChat.Models
{
    public class AppConfig : INotifyPropertyChanged
    {
        public string ApiHost { get; set; } = "openaiapi.elecho.top";
        public string ApiKey { get; set; } = string.Empty;
        public string ApiGptModel { get; set; } = "gpt-3.5-turbo";
        public int ApiTimeout { get; set; } = 5000;

        public string[] SystemMessages { get; set; } = new string[]
        {

        };

        public bool UseEnterToSendMessage { get; set; } = false;

        public bool WindowAlwaysOnTop { get; set; } = false;


        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
