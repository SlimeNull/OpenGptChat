using LiteDB;
using OpenGptChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGptChat.Services
{
    public class ChatStorageService : IDisposable
    {
        public ChatStorageService(
            ConfigurationService configurationService)
        {
            ConfigurationService = configurationService;

            Database = new LiteDatabase(
                new ConnectionString()
                {
                    Filename = configurationService.Configuration.ChatStoragePath,
                    
                });

            ChatSessions = Database.GetCollection<ChatSession>();
            ChatMessages = Database.GetCollection<ChatMessage>();
        }

        private ILiteCollection<ChatSession> ChatSessions { get; }
        private ILiteCollection<ChatMessage> ChatMessages { get; }


        public LiteDatabase Database { get; }
        public ConfigurationService ConfigurationService { get; }

        public IEnumerable<ChatSession> GetAllSessions()
        {
            return ChatSessions.FindAll();
        }

        public ChatStorageService SaveNewSession(string name)
        {
            ChatSession session = ChatSession.Create(name);

            return SaveSession(session);
        }

        public ChatStorageService SaveSession(ChatSession session)
        {
            if (!ChatSessions.Update(session.Id, session))
                ChatSessions.Insert(session.Id, session);

            return this;
        }

        public bool DeleteSession(Guid id)
        {
            return ChatSessions.DeleteMany(session => session.Id == id) > 0;
        }

        public IEnumerable<ChatMessage> GetAllMessages(Guid sessionId)
        {
            return ChatMessages.Query()
                .Where(msg => msg.SessionId == sessionId)
                .OrderBy(msg => msg.Timestamp)
                .ToEnumerable();
        }

        public ChatStorageService SaveNewMessage(Guid sessionId, string role, string content)
        {
            ChatMessage message = ChatMessage.Create(sessionId, role, content);

            return SaveMessage(message);
        }

        public ChatStorageService SaveMessage(ChatMessage message)
        {
            if (!ChatMessages.Update(message.Id, message))
                ChatMessages.Insert(message.Id, message);

            return this;
        }

        public bool ClearMessage(Guid sessionId)
        {
            return ChatMessages.DeleteMany(msg => msg.SessionId == sessionId) > 0;
        }



        bool disposed = false;
        public void Dispose()
        {
            if (disposed)
                return;

            Database.Dispose();
            disposed = true;
        }
    }
}
