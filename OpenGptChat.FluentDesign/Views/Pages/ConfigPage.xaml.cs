using OpenGptChat.Abstraction;
using OpenGptChat.ViewModels;
using Wpf.Ui.Common.Interfaces;

namespace OpenGptChat.FluentDesign.Views.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class ConfigPage : INavigableView<ConfigPageModel>, IConfigPage
    {
        public ConfigPage(ConfigPageModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

        public ConfigPageModel ViewModel { get; }
    }
}