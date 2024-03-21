using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenGptChat.Models;

namespace OpenGptChat.ViewModels
{
    public partial class MainPageViewModel : ViewModelBase
    {
        public ObservableCollection<ChatSession> Sessions { get; } = new();

        [ObservableProperty]
        private ChatSession? _selectedSession;

        [ObservableProperty]
        private string _textInput = string.Empty;

        [RelayCommand]
        public async Task Send()
        {
#if DEBUG
            if (SelectedSession is not ChatSession selectedSession)
                return;
            if (string.IsNullOrWhiteSpace(TextInput))
                return;

            selectedSession.Messages.Add(
                new ChatMessage()
                {
                    Title = "Me",
                    MessageText = TextInput,
                });

            TextInput = string.Empty;
#endif
        }
    }
}
