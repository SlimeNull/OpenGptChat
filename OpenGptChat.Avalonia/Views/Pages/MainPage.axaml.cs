using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using OpenGptChat.Models;
using OpenGptChat.ViewModels;

namespace OpenGptChat.Views.Pages;

public partial class MainPage : UserControl
{
    public MainPage()
    {
        DataContext = new MainPageViewModel();

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
                        Role = Role.User,
                        Sender = "DebugTestMessage",
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
                        Role = Role.Assistant,
                        Sender = "Assistant",
                        MessageText =
                            """
                            Some text

                            1. List item 1
                            2. List item 2
                            3. List item 3
                            """
                    },
                    new ChatMessage()
                    {
                        Role = Role.User,
                        Sender = "slime",
                        MessageText =
                            """
                            1. Complex list item
                               The second line
                            2. Complex list item
                               The second line `code inline`, *italic*, **bold**

                               - ajwoeif
                               - jwoeifjwoe
                            """
                    },
                    new ChatMessage()
                    {
                        Role = Role.Assistant,
                        Sender = "Assistant",
                        MessageText = "Some text, **bold**, *italic*, `code inline`"
                    },
                    new ChatMessage()
                    {
                        Role = Role.User,
                        Sender = "slime",
                        MessageText =
                            """
                            A very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very very long message
                            """
                    },
                }
            });
#endif

        InitializeComponent();
    }

    private void GoToConfigPageButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var mainWindow = App.Services.GetRequiredService<MainWindow>();
        mainWindow.Content = App.Services.GetRequiredService<ConfigPage>();
    }
}