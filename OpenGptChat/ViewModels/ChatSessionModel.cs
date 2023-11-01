using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenGptChat.Models;
using OpenGptChat.Services;
using OpenGptChat.Utilities;
using OpenGptChat.Views;
using OpenGptChat.Views.Dialogs;
using OpenGptChat.Views.Pages;

namespace OpenGptChat.ViewModels
{
    public partial class ChatSessionModel : ObservableObject
    {
        public ChatSessionModel(ChatSession storage)
        {
            Storage = storage;
            SetupStorage(storage);
        }

        private void SetupStorage(ChatSession storage)
        {
#pragma warning disable MVVMTK0034 // Direct field reference to [ObservableProperty] backing field
            _id = storage.Id;
            _name = storage.Name;
            _enableChatContext = storage.EnableChatContext;
            _systemMessages = storage.SystemMessages.WrapCollection();
#pragma warning restore MVVMTK0034 // Direct field reference to [ObservableProperty] backing field
        }

        public ChatSession? Storage
        {
            get => storage; set
            {
                storage = value;

                if (value != null)
                    SetupStorage(value);
            }
        }


        [ObservableProperty]
        private Guid _id;

        [ObservableProperty]
        private string? _name = string.Empty;

        [ObservableProperty]
        private bool? _enableChatContext = null;

        [ObservableProperty]
        private ObservableCollection<ValueWrapper<string>> _systemMessages
            = new ObservableCollection<ValueWrapper<string>>();




        [ObservableProperty]
        [NotifyPropertyChangedFor(
            nameof(IsReadOnly))]
        private bool _isEditing = false;
        private ChatSession? storage;

        public bool IsReadOnly => !IsEditing;


        public ChatPage Page => ChatPageService.GetPage(Id);
        public ChatPageModel PageModel => Page.ViewModel;


        private static ChatPageService ChatPageService { get; } =
            App.GetService<ChatPageService>();
        private static ChatStorageService ChatStorageService { get; } =
            App.GetService<ChatStorageService>();

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            SyncStorage();
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

        [RelayCommand]
        public void Config()
        {
            ChatSessionConfigDialog dialog =
                new ChatSessionConfigDialog(this);

            if (Application.Current.MainWindow is Window window)
                dialog.Owner = window;

            if (dialog.ShowDialog() ?? false)
                SyncStorage();
        }

        [RelayCommand]
        public void SyncStorage()
        {
            if (Storage != null)
            {
                Storage = Storage with
                {
                    Name = Name,
                    EnableChatContext = EnableChatContext,
                    SystemMessages = SystemMessages.UnwrapToArray()
                };

                ChatStorageService.SaveSession(Storage);
            }
        }
    }
}
