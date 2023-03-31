using OpenGptChat.ViewModels;

namespace OpenGptChat.Abstraction
{
    public interface IAppWindow
    {
        public AppWindowModel ViewModel { get; }

        public void Show();
        public void Navigate<TPage>() where TPage : class;
    }
}