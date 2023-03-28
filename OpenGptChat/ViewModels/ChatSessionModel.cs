using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenGptChat.Models;
using OpenGptChat.Services;
using OpenGptChat.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public bool IsReadOnly => !_isEditing;


        public ChatPage Page => ChatPageService.GetPage(_id);
        public ChatPageModel PageModel => Page.ViewModel;


        private static ChatPageService ChatPageService { get; } =
            App.GetService<ChatPageService>();
        private static ChatStorageService ChatStorageService { get; } =
            App.GetService<ChatStorageService>();

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (Storage != null)
            {
                Storage = Storage with
                {
                    Name = _name
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
