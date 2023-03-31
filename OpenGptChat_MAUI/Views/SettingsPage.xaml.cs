using OpenGptChat_MAUI.ViewModels;

namespace OpenGptChat_MAUI.Views;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsPageModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected override bool OnBackButtonPressed()
    {
        Shell.Current.GoToAsync("//MainPage").ConfigureAwait(false);
        return ! base.OnBackButtonPressed();
    }
    
}