using OpenGptChat_MAUI.ViewModels;

namespace OpenGptChat_MAUI
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage(MainPageModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}