using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenGptChat.Views;
using OpenGptChat.Views.Pages;

namespace OpenGptChat.ViewModels
{
    public partial class MainPageModel : ObservableObject
    {
        [ObservableProperty]
        private ChatPage? _currentChat;
    }
}
