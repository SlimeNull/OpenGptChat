using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenGptChat.Views;

namespace OpenGptChat.ViewModels
{
    public partial class MainPageModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ChatSessionModel> _sessions =
            new ObservableCollection<ChatSessionModel>();

        [ObservableProperty]
        private ChatSessionModel? _selectedSession;


        [ObservableProperty]
        private ChatPage? _currentChat;
    }
}
