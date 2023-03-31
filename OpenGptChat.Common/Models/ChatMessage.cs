using LiteDB;
using System;

namespace OpenGptChat.Models
{
    public record class ChatMessage
    {
        public ChatMessage(Guid id, Guid sessionId, string role, string content, DateTime timestamp)
        {
            this.Id = id;
            this.SessionId = sessionId;
            this.Role = role;
            this.Content = content;
            this.Timestamp = timestamp;
        }

        [BsonId]
        public Guid Id { get; }
        public Guid SessionId { get; }
        public string Role { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; }

        public static ChatMessage Create(Guid sessionId, string role, string content) => 
            new ChatMessage(Guid.NewGuid(), sessionId, role, content, DateTime.Now);
    }
}
