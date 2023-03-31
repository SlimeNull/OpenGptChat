using OpenGptChat.ViewModels;

namespace OpenGptChat.Abstraction
{
    public interface IChatPage : IPage
    {
        public ChatPageModel ViewModel { get; }

        public void InitSession(Guid sessionId);
    }
}