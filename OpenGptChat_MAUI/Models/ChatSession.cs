using LiteDB;
using System;

namespace OpenGptChat_MAUI.Models
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

        public static ChatSession Create(string name) => 
            new ChatSession(Guid.NewGuid(), name);
    }
}
