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
                        MessageText =
                            """
                            # Header1

                            ## Header2

                            ### Header3

                            #### Header4

                            ##### Header5

                            ###### Header6
                            """
                    },
                    new ChatMessage()
                    {
                        Title = "Bot",
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
                        Title = "Me",
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
                        Title = "Bot",
                        MessageText = "Some text, **bold**, *italic*, `code inline`"
                    },
                }
            });
#endif

        InitializeComponent();
    }
}