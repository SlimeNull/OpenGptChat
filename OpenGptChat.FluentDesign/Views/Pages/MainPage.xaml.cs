using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenGptChat.Abstraction;
using OpenGptChat.ViewModels;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls;

namespace OpenGptChat.FluentDesign.Views.Pages
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class MainPage : UiPage, INavigableView<MainPageModel>, IMainPage
    {
        public MainPage(MainPageModel viewModel)
        {
            ViewModel = viewModel;
            
            InitializeComponent();
        }

        public MainPageModel ViewModel { get; }

        [RelayCommand]
        public void ClickButton()
        {
            
        }
    }
}