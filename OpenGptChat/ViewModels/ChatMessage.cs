using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace OpenGptChat.ViewModels
{
    public class ChatMessage : INotifyPropertyChanged
    {
        public string Username { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;



        [DependsOn(nameof(Username))]
        public bool IsMe => "Me".Equals(Username, StringComparison.CurrentCultureIgnoreCase);

        [DependsOn(nameof(IsMe))]
        public HorizontalAlignment SelfAlignment => IsMe ? HorizontalAlignment.Right : HorizontalAlignment.Left;

        [DependsOn(nameof(IsMe))]
        public CornerRadius SelfCornorRadius => IsMe ? new CornerRadius(5, 0, 5, 5) : new CornerRadius(0, 5, 5, 5);

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
