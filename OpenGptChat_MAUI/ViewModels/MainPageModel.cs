using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGptChat_MAUI.ViewModels
{
    public partial class MainPageModel:ObservableObject
    {
        [RelayCommand]
        public async Task GoToSettings()
        {
            await Shell.Current.GoToAsync("//SettingsPage");
        }
    }
}
