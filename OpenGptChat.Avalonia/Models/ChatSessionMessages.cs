using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace OpenGptChat.Models
{
    public class ChatSessionMessages : ObservableCollection<ChatMessage>
    {
        public ChatSession Owner { get; }
        public ChatMessage? LastMessage => this.LastOrDefault();

        public ChatSessionMessages(ChatSession owner)
        {
            Owner = owner;
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);

            OnPropertyChanged(new PropertyChangedEventArgs(nameof(LastMessage)));
        }
    }
}
