using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenGptChat_MAUI.Utilities;
using System.Collections.ObjectModel;

namespace OpenGptChat_MAUI.ViewModels
{
    public partial class SettingsPageModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ValueWrapper<string>> _systemMessages =
                new ObservableCollection<ValueWrapper<string>>();

        [RelayCommand]
        public async void Back()
        {

            await Shell.Current.GoToAsync("//MainPage");
        }
    }
}
