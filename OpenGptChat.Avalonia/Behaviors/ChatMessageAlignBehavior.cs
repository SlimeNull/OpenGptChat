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

        protected override void OnAttachedToVisualTree()
        {
            base.OnAttachedToVisualTree();

            if (AssociatedObject is not ChatMessage chatMessage)
                return;

            if (chatMessage.Title == LeftAlignmentTitle)
                chatMessage.HorizontalAlignment = HorizontalAlignment.Left;
            else if (chatMessage.Title == RightAlignmentTitle)
                chatMessage.HorizontalAlignment = HorizontalAlignment.Right;
        }
    }
}
