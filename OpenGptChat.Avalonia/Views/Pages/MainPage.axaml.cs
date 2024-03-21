using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OpenGptChat.Models;
using OpenGptChat.ViewModels;

namespace OpenGptChat.Views.Pages;

public partial class MainPage : UserControl
{
    public MainPage()
    {
        DataContext = new MainPageViewModel();

        InitializeComponent();

#if DEBUG
        var vm = (MainPageViewModel)DataContext;

        vm.Sessions.Add(
            new ChatSession()
            {
                Title = "DebugTestSession",
                Messages =
                {
                    new ChatMessage()
                    {
                        Title = "DebugTestMessage",
                        MessageText = "Some text, **bold**, *italic*, `code inline`"
                    }
                }
            });

        vm.Sessions.Add(
            new ChatSession()
            {
                Title = "DebugTestSession",
                Messages =
                {
                    new ChatMessage()
                    {
                        Title = "Bot",
                        MessageText = "Some text, **bold**, *italic*, `code inline`"
                    },
                    new ChatMessage()
                    {
                        Title = "Me",
                        MessageText = "Some text, **bold**, *italic*, `code inline`"
                    },
                    new ChatMessage()
                    {
                        Title = "Bot",
                        MessageText = "Some text, **bold**, *italic*, `code inline`"
                    },
                    new ChatMessage()
                    {
                        Title = "Me",
                        MessageText = "Some text, **bold**, *italic*, `code inline`"
                    },
                    new ChatMessage()
                    {
                        Title = "Bot",
                        MessageText = "Some text, **bold**, *italic*, `code inline`"
                    },
                }
            });
#endif
    }
}