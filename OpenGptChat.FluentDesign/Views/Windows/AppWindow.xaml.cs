using OpenGptChat.Abstraction;
using OpenGptChat.FluentDesign.Views.Pages;
using OpenGptChat.Services;
using OpenGptChat.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace OpenGptChat.FluentDesign.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AppWindow : INavigationWindow, IAppWindow
    {
        public AppWindow(
            AppWindowModel viewModel,
            NoteService noteService,
            IPageService pageService)
        {
            ViewModel = viewModel;
            PageService = pageService;
            NoteService = noteService;
            DataContext = this;

            InitializeComponent();

            RootNavigation.PageService = pageService;
        }
        public AppWindowModel ViewModel { get; }
        public NoteService NoteService { get; }
        public IPageService PageService { get; }

        public Frame GetFrame() => RootFrame;
        public INavigation GetNavigation() => RootNavigation;

        public void Navigate<TPage>() 
            where TPage : class
        {
            RootNavigation.Navigate(typeof(TPage));
        }

        public bool Navigate(Type pageType) =>
            RootNavigation.Navigate(pageType);
        public void SetPageService(IPageService pageService) =>
            RootNavigation.PageService = pageService;
        public void ShowWindow() => 
            Show();
        public void CloseWindow() => 
            Close();

        /// <summary>
        /// Raises the closed event.
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Make sure that closing this window will begin the process of closing the application.
            Application.Current.Shutdown();
        }
    }
}