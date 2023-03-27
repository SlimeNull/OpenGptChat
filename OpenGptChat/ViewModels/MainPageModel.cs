using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGptChat.ViewModels
{
    public partial class MainPageModel : ObservableObject
    {
        [ObservableProperty]
        private string _inputBoxText = string.Empty;

        [ObservableProperty]
        private ObservableCollection<ChatMessage> _messages = 
            new ObservableCollection<ChatMessage>();
    }
}
