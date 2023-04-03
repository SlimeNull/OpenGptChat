using CommunityToolkit.Mvvm.ComponentModel;

namespace OpenGptChat.ViewModels
{
    public partial class NoteDataModel : ObservableObject
    {
        [ObservableProperty]
        private string _text = string.Empty;

        [ObservableProperty]
        private bool _show = false;
    }
}
