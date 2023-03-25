using OpenChat.Models;
using OpenChat.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OpenChat.ViewModels
{
    public class MainPageModel : INotifyPropertyChanged
    {
        public MainPageModel(ConfigurationService configurationService)
        {
            SendMessageModifierKey = configurationService.Configuration.UseEnterToSendMessage ?
                ModifierKeys.None : ModifierKeys.Control;

            SendMessageToolTip = !configurationService.Configuration.UseEnterToSendMessage ?
                "Send message (Ctrl + Enter)" : "Send message (Enter)";
        }

        public string InputBoxText { get; set; } = string.Empty;

        public ObservableCollection<ChatMessage> Messages { get; } = new ObservableCollection<ChatMessage>();

        public ModifierKeys SendMessageModifierKey { get; protected set; }

        public string SendMessageToolTip { get; protected set; } = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
