using OpenGptChat.ViewModels;

namespace OpenGptChat.Abstraction
{
    public interface IMainPage : IPage
    {
        public MainPageModel ViewModel { get; }
    }
}