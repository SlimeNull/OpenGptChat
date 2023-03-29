using CommunityToolkit.Mvvm.Input;
using OpenGptChat.Services;
using OpenGptChat.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace OpenGptChat
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
            DataContext = this;

            InitializeComponent();
        }

        public NoteService NoteService { get; }

        public AppWindowModel ViewModel { get; }
        public NoteDataModel NoteDataModel => NoteService.Data;

        public void Navigate(object content) => appFrame.Navigate(content);


        [RelayCommand]
        public void CloseNote()
        {
            NoteService.Close();
        }

        [RelayCommand]
        public void ShowApp()
        {
            this.Show();

            if (this.WindowState == WindowState.Minimized)
                this.WindowState = WindowState.Normal;

            if (!this.IsActive)
                this.Activate();
        }

        [RelayCommand]
        public void HideApp()
        {
            this.Hide();
        }

        [RelayCommand]
        public void CloseApp()
        {
            Application.Current.Shutdown();
        }

        private void NoteControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NoteService.Close();
        }
    }
}
