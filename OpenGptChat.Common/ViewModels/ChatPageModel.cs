using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGptChat.ViewModels
{
    public partial class ChatPageModel : ObservableObject
    {
        public ChatPageModel()
        {
            _messages.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(LastMessage));
            };
        }

        [ObservableProperty]
        private string _inputBoxText = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(
            nameof(LastMessage))]
        private ObservableCollection<ChatMessageModel> _messages =
            new ObservableCollection<ChatMessageModel>();

        public ChatMessageModel? LastMessage => Messages.Count > 0 ? Messages.Last() : null;
    }
}
