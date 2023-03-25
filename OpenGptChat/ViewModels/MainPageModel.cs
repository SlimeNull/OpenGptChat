using OpenGptChat.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGptChat.ViewModels
{
    public class MainPageModel : INotifyPropertyChanged
    {
        public string InputBoxText { get; set; } = string.Empty;

        public ObservableCollection<ChatMessage> Messages { get; } = new ObservableCollection<ChatMessage>();

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
