using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

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
