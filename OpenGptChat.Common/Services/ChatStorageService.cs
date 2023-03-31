using LiteDB;
using OpenGptChat.Models;

namespace OpenGptChat.Services
{
    public class ChatStorageService : IDisposable
    {
        public ChatStorageService(
            ConfigurationService configurationService)
        {
            ConfigurationService = configurationService;
        }

        private ILiteCollection<ChatSession>? ChatSessions { get; set; }
        private ILiteCollection<ChatMessage>? ChatMessages { get; set; }


        public LiteDatabase? Database { get; private set; }
        public ConfigurationService ConfigurationService { get; }

        public IEnumerable<ChatSession> GetAllSessions()
        {
            if (ChatSessions == null)
                throw new InvalidOperationException("Not initialized");

            return ChatSessions.FindAll();
        }

        public ChatStorageService SaveNewSession(string name)
        {
            ChatSession session = ChatSession.Create(name);

            return SaveSession(session);
        }

        public ChatStorageService SaveSession(ChatSession session)
        {
            if (ChatSessions == null)
                throw new InvalidOperationException("Not initialized");

            if (!ChatSessions.Update(session.Id, session))
                ChatSessions.Insert(session.Id, session);

            return this;
        }

        public bool DeleteSession(Guid id)
        {
            if (ChatSessions == null)
                throw new InvalidOperationException("Not initialized");

            return ChatSessions.DeleteMany(session => session.Id == id) > 0;
        }

        public IEnumerable<ChatMessage> GetAllMessages(Guid sessionId)
        {
            if (ChatMessages == null)
                throw new InvalidOperationException("Not initialized");

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
            if (ChatMessages == null)
                throw new InvalidOperationException("Not initialized");

            if (!ChatMessages.Update(message.Id, message))
                ChatMessages.Insert(message.Id, message);

            return this;
        }

        public bool ClearMessage(Guid sessionId)
        {
            if (ChatMessages == null)
                throw new InvalidOperationException("Not initialized");

            return ChatMessages.DeleteMany(msg => msg.SessionId == sessionId) > 0;
        }


        public void Initialize()
        {
            Database = new LiteDatabase(
                new ConnectionString()
                {
                    Filename = ConfigurationService.Configuration.ChatStoragePath,

                });

            ChatSessions = Database.GetCollection<ChatSession>();
            ChatMessages = Database.GetCollection<ChatMessage>();
        }


        bool disposed = false;
        public void Dispose()
        {
            if (disposed)
                return;

            Database?.Dispose();
            disposed = true;
        }
    }
}
