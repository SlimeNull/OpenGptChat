using System;
using System.ComponentModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenGptChat.Models;
using OpenGptChat.Services;

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
        [NotifyPropertyChangedFor(
            nameof(SingleLineContent))]
        private string _content = string.Empty;




        public string SingleLineContent => (Content ?? string.Empty).Replace('\n', ' ').Replace('\r', ' ');

        [ObservableProperty]
        [NotifyPropertyChangedFor(
            nameof(IsReadOnly))]
        private bool _isEditing = false;

        public bool IsReadOnly => !IsEditing;


        private static ChatStorageService ChatStorageService { get; } =
            App.GetService<ChatStorageService>();



        // 用于将数据同步到数据存储
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            // 如果有后备存储, 则使用存储服务保存
            if (Storage != null)
            {
                Storage = Storage with
                {
                    Role = Role,
                    Content = Content
                };

                ChatStorageService.SaveMessage(Storage);
            }
        }







        #region 布局用的一些属性

        public string DisplayName => string.Equals(Role, "user", StringComparison.CurrentCultureIgnoreCase) ? "Me" : "Bot";

        public bool IsMe => "Me".Equals(DisplayName, StringComparison.CurrentCultureIgnoreCase);

        public HorizontalAlignment SelfAlignment => IsMe ? HorizontalAlignment.Right : HorizontalAlignment.Left;

        public CornerRadius SelfCornorRadius => IsMe ? new CornerRadius(5, 0, 5, 5) : new CornerRadius(0, 5, 5, 5);

        #endregion



        #region 页面用的一些指令

        [RelayCommand]
        public void Copy()
        {
            Clipboard.SetText(Content);
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
