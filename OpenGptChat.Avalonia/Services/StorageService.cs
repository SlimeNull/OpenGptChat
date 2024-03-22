using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using OpenGptChat.Models;

namespace OpenGptChat.Services
{
    public class StorageService
    {
        public void SaveSession(ChatSession session)
        {

        }

        public void PopulateSession(ChatSession session)
        {

        }

        class DbChatSession
        {
            [BsonId]
            public int Id { get; set; }

            public string Name { get; set; } = string.Empty;

            public bool EnablContext { get; set; }
            public string[]? SystemMessages { get; set; }
        }

        class DbChatMessage
        {
            [BsonId]
            public int Id { get; set; }

            public int SessionId { get; set; }

            public string Sender { get; set; } = string.Empty;
            public string Content { get; set; } = string.Empty;
        }
    }
}
