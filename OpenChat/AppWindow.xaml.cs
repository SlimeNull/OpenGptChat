using CommunityToolkit.Mvvm.Input;
using OpenChat.Models;
using OpenChat.Services;
using OpenChat.ViewModels;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpenChat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AppWindow : Window
    {
        public AppWindow(
            AppWindowModel viewModel,
            NoteService noteService)
        {
            ViewModel = viewModel;
            NoteService = noteService;
            InitializeComponent();

            DataContext = this;
        }

        public NoteService NoteService { get; }

        public AppWindowModel ViewModel { get; }
        public NoteData NoteDataModel => NoteService.Data;

        public void Navigate(object content) => appFrame.Navigate(content);


        [RelayCommand]
        public void CloseNote()
        {
            NoteService.Close();
        }

        private void NoteControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NoteService.Close();
        }
    }
}
