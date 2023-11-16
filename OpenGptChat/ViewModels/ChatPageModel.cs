using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenGptChat.Services;

namespace OpenGptChat.ViewModels
{
    public partial class ChatPageModel : ObservableObject
    {
        public ChatPageModel(
            ChatStorageService chatStorageService)
        {
            _chatStorageService = chatStorageService;

            Messages.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(LastMessage));
            };
        }

        [ObservableProperty]
        private string _inputBoxText = string.Empty;
        private readonly ChatStorageService _chatStorageService;

        public ObservableCollection<ChatMessageModel> Messages { get; } = new();

        public ChatMessageModel? LastMessage => Messages.Count > 0 ? Messages.Last() : null;


        [RelayCommand]
        public void DeleteMessage(ChatMessageModel messageModel)
        {
            Messages.Remove(messageModel);

            if (messageModel.Storage != null)
                _chatStorageService.DeleteMessage(messageModel.Storage);
        }


        [RelayCommand]
        public void DeleteMessagesAbove(ChatMessageModel messageModel)
        {
            while (true)
            {
                int index = Messages.IndexOf(messageModel);
                if (index <= 0)
                    break;

                Messages.RemoveAt(0);
            }

            if (messageModel.Storage != null)
                _chatStorageService.DeleteMessagesBefore(messageModel.Storage.SessionId, messageModel.Storage.Timestamp);
        }

        [RelayCommand]
        public void DeleteMessagesBelow(ChatMessageModel messageModel)
        {
            while (true)
            {
                int index = Messages.IndexOf(messageModel);
                if (index == -1 || index == Messages.Count - 1)
                    break;

                Messages.RemoveAt(index + 1);
            }

            if (messageModel.Storage != null)
                _chatStorageService.DeleteMessagesAfter(messageModel.Storage.SessionId, messageModel.Storage.Timestamp);
        }
    }
}
