using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenGptChat.Models;
using OpenGptChat.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace OpenGptChat.ViewModels
{
    public partial class ChatMessageModel : ObservableObject
    {
        public ChatMessageModel(string role, string content)
        {
            _role = role;
            _content = content;
        }

        public ChatMessageModel(ChatMessage storage)
        {
            Storage = storage;

            _role = storage.Role;
            _content = storage.Content;
        }

        public ChatMessage? Storage { get; set; }

        [ObservableProperty]
        private string _role = "user";

        [ObservableProperty]
        private string _content = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(
            nameof(IsReadOnly))]
        private bool _isEditing = false;

        public bool IsReadOnly => !_isEditing;


        private static ChatStorageService ChatStorageService { get; } =
            App.GetService<ChatStorageService>();

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            // 如果有后备存储, 则使用存储服务保存
            if (Storage != null)
            {
                Storage = Storage with
                {
                    Role = _role,
                    Content = _content
                };

                ChatStorageService.SaveMessage(Storage);
            }
        }







        #region Layout properties

        public string DisplayName => string.Equals(_role, "user", StringComparison.CurrentCultureIgnoreCase) ? "Me" : "Bot";

        public bool IsMe => "Me".Equals(DisplayName, StringComparison.CurrentCultureIgnoreCase);

        public HorizontalAlignment SelfAlignment => IsMe ? HorizontalAlignment.Right : HorizontalAlignment.Left;

        public CornerRadius SelfCornorRadius => IsMe ? new CornerRadius(5, 0, 5, 5) : new CornerRadius(0, 5, 5, 5);

        #endregion



        #region Layout commands

        [RelayCommand]
        public void Copy()
        {
            Clipboard.SetText(_content);
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

        #endregion
    }
}
