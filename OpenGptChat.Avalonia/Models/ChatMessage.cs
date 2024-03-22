using CommunityToolkit.Mvvm.ComponentModel;
using LiteDB;

namespace OpenGptChat.Models
{
    public partial class ChatMessage : ObservableObject
    {
        [ObservableProperty]
        private Role _role;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Overview))]
        private string _sender = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Overview))]
        private string _messageText = string.Empty;

        [BsonIgnore]
        public string Overview => $"{Sender}: {MessageText}";
    }
}
