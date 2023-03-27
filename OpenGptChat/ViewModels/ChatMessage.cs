using CommunityToolkit.Mvvm.ComponentModel;
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
    public partial class ChatMessage : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(
            nameof(IsMe),
            nameof(SelfAlignment),
            nameof(SelfCornorRadius))]
        public string _username = string.Empty;

        [ObservableProperty]
        public string _message = string.Empty;

        public bool IsMe => "Me".Equals(Username, StringComparison.CurrentCultureIgnoreCase);

        public HorizontalAlignment SelfAlignment => IsMe ? HorizontalAlignment.Right : HorizontalAlignment.Left;

        public CornerRadius SelfCornorRadius => IsMe ? new CornerRadius(5, 0, 5, 5) : new CornerRadius(0, 5, 5, 5);
    }
}
