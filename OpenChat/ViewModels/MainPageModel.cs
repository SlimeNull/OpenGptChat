using OpenChat.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenChat.ViewModels
{
    public class MainPageModel : INotifyPropertyChanged
    {
        public string InputBoxText { get; set; } = string.Empty;
        public bool InputBoxAvailable { get; set; } = true;

        public ObservableCollection<ChatMessage> Messages { get; } = new ObservableCollection<ChatMessage>();

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
