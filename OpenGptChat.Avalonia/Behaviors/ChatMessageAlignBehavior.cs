using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Layout;
using Avalonia.Xaml.Interactivity;
using OpenGptChat.Views.Controls;

namespace OpenGptChat.Behaviors
{
    public class ChatMessageAlignBehavior : Behavior<ChatMessage>
    {
        public string? LeftAlignmentTitle { get; set; }
        public string? RightAlignmentTitle { get; set; }

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject is not ChatMessage chatMessage)
                return;

            // TODO: 这里无法获取最新属性, 导致对齐方式赋值不正确
            if (chatMessage.Title == LeftAlignmentTitle)
                chatMessage.HorizontalAlignment = HorizontalAlignment.Left;
            else if (chatMessage.Title == RightAlignmentTitle)
                chatMessage.HorizontalAlignment = HorizontalAlignment.Right;
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change) => base.OnPropertyChanged(change);
    }
}
