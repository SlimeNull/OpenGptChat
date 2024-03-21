using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace OpenGptChat.Models
{
    public partial class ChatSession : ObservableObject
    {
        [ObservableProperty]
        private string _title = string.Empty;

        public ChatSessionMessages Messages { get; } 

        public ChatSession()
        {
            Messages = new ChatSessionMessages(this);
        }
    }
}
