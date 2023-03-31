using OpenGptChat.Abstraction;
using OpenGptChat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Controls;

namespace OpenGptChat.FluentDesign.Views.Pages
{
    /// <summary>
    /// Interaction logic for ChatPage.xaml
    /// </summary>
    public partial class ChatPage : UiPage, IChatPage
    {
        public ChatPage(
            ChatPageModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

        public ChatPageModel ViewModel { get; }

        public void InitSession(Guid sessionId) => throw new NotImplementedException();
    }
}
