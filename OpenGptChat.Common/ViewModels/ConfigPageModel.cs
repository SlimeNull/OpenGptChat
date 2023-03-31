using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenGptChat.Models;

namespace OpenGptChat.ViewModels
{
    public partial class ConfigPageModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ValueWrapper<string>> _systemMessages =
            new ObservableCollection<ValueWrapper<string>>();
    }
}
