using CommunityToolkit.Mvvm.ComponentModel;
using LiteDB;

namespace OpenGptChat.Models
{
    public partial class ChatMessage : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Overview))]
        private string _title = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Overview))]
        private string _messageText = string.Empty;

        [BsonIgnore]
        public string Overview => $"{Title}: {MessageText}";
    }
}
