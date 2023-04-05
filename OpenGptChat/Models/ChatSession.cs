using System;
using System.Windows.Documents;
using LiteDB;

namespace OpenGptChat.Models
{
    public record class ChatSession
    {
        public ChatSession(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        [BsonId]
        public Guid Id { get; }
        public string Name { get; set; }



        public string[] SystemMessages { get; set; } = Array.Empty<string>();
        public bool? EnableChatContext { get; set; } = null;



        public static ChatSession Create(string name) => 
            new ChatSession(Guid.NewGuid(), name);
    }
}
