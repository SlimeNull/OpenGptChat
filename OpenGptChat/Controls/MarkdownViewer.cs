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
using Markdig;
using OpenGptChat.Markdown;

namespace OpenGptChat.Controls
{
    public class MarkdownViewer : Control
    {
        static MarkdownViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MarkdownViewer), new FrameworkPropertyMetadata(typeof(MarkdownViewer)));
        }



        public string Content
        {
            get { return (string)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public FrameworkElement RenderedContent
        {
            get { return (FrameworkElement)GetValue(RenderedContentProperty); }
            private set { SetValue(RenderedContentProperty, value); }
        }



        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(string), typeof(MarkdownViewer), new PropertyMetadata(string.Empty, ContentChangedCallback));

        // Using a DependencyProperty as the backing store for RenderedContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RenderedContentProperty =
            DependencyProperty.Register(nameof(RenderedContent), typeof(FrameworkElement), typeof(MarkdownViewer), new PropertyMetadata(null));



        private static void ContentChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not MarkdownViewer markdownViewer)
                return;

            var doc = Markdig.Markdown.Parse(
                markdownViewer.Content,
                new MarkdownPipelineBuilder()
                    .UseEmphasisExtras()
                    .UseGridTables()
                    .UsePipeTables()
                    .UseTaskLists()
                    .UseAutoLinks()
                    .Build());

            var renderer =
                App.GetService<MarkdownWpfRenderer>();

            var markdownWpfElement =
                renderer.RenderDocument(doc);

            if (markdownWpfElement != null)
                markdownViewer.RenderedContent = markdownWpfElement;
        }
    }
}
