using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenGptChat.Models;

namespace OpenGptChat.ViewModels
{
    public partial class MainPageViewModel : ViewModelBase
    {
        public ObservableCollection<ChatSession> Sessions { get; } = new();

        [ObservableProperty]
        private ChatSession? selectedSession;
    }
}
