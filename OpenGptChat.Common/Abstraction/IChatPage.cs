using OpenGptChat.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace OpenGptChat.Abstraction
{
    public interface IChatPage : IPage
    {
        public ChatPageModel ViewModel { get; }

        public void InitSession(Guid sessionId);
    }
}