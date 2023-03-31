using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenGptChat.Abstraction;
using OpenGptChat.Models;
using OpenGptChat.Services;

namespace OpenGptChat.ViewModels
{
    public partial class ChatSessionModel : ObservableObject
    {
        public ChatSessionModel(Guid id, string name)
        {
            _id = id;
            _name = name;
        }

        public ChatSessionModel(ChatSession storage)
        {
            Storage = storage;

            _id = storage.Id;
            _name = storage.Name;
        }

        public ChatSession? Storage { get; set; }


        [ObservableProperty]
        private Guid _id;

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(
            nameof(IsReadOnly))]
        private bool _isEditing = false;

        public bool IsReadOnly => !IsEditing;


        public IChatPage Page => ChatPageService.GetPage(Id);
        public ChatPageModel PageModel => Page.ViewModel;


        private static ChatPageService ChatPageService { get; } =
            GlobalServices.GetService<ChatPageService>();
        private static ChatStorageService ChatStorageService { get; } =
            GlobalServices.GetService<ChatStorageService>();

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (Storage != null)
            {
                Storage = Storage with
                {
                    Name = Name
                };

                ChatStorageService.SaveSession(Storage);
            }
        }

        [RelayCommand]
        public void StartEdit()
        {
            IsEditing = true;
        }

        [RelayCommand]
        public void EndEdit()
        {
            IsEditing = false;
        }
    }
}
