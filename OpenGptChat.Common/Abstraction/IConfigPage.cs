using OpenGptChat.ViewModels;

namespace OpenGptChat.Abstraction
{
    public interface IConfigPage : IPage
    {
        public ConfigPageModel ViewModel { get; }
    }
}