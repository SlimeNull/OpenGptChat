using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace OpenGptChat.Models
{
    public partial class ChatSession : ObservableObject
    {
        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private string _title = string.Empty;

        public ChatSessionMessages Messages { get; } 
        public ChatSessionMessages SystemMessages { get; }

        public ChatSession()
        {
            Messages = new ChatSessionMessages(this);
            SystemMessages = new ChatSessionMessages(this);
        }
    }
}
