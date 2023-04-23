using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ColorCode;
using ColorCode.Common;
using ColorCode.Parsing;
using ColorCode.Styling;
using Markdig.Extensions.Tables;
using Markdig.Extensions.TaskLists;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using OpenGptChat.Common.Models;
using OpenGptChat.Services;
using WpfDocs = System.Windows.Documents;

namespace OpenGptChat.Markdown
{

    public class MarkdownWpfRenderer
    {
        public double Heading1Size = 24;
        public double Heading2Size = 18;
        public double Heading3Size = 16;
        public double Heading4Size = 14;
        public double NormalSize = 12;

        public ColorMode ColorMode { get; set; } = App.GetService<ColorModeService>().CurrentActualMode;






        public static event EventHandler<MarkdownLinkNavigateEventArgs>? LinkNavigate;

        public FrameworkElement RenderDocument(MarkdownDocument document)
        {
            StackPanel documentElement = new StackPanel();

            foreach (var renderedBlock in RenderBlocks(document))
                documentElement.Children.Add(renderedBlock);

            return documentElement;
        }

        public List<FrameworkElement> RenderBlocks(IEnumerable<Block> blocks)
        {
            List<FrameworkElement> elements = new List<FrameworkElement>();
            FrameworkElement? tailElement = null;

            foreach (var block in blocks)
            {
                FrameworkElement? renderedBlock = RenderBlock(block);

                if (renderedBlock != null)
                {
                    elements.Add(renderedBlock);
                    tailElement = renderedBlock;
                }
            }

            if (tailElement != null)
                tailElement.Margin = tailElement.Margin with
                {
                    Bottom = 0
                };

            return elements;
        }

        public FrameworkElement RenderBlock(Block block)
        {

            if (block is ParagraphBlock paragraphBlock)
            {
                return RenderParagraphBlock(paragraphBlock);
            }
            else if (block is HeadingBlock headingBlock)
            {
                return RenderHeadingBlock(headingBlock);
            }
            else if (block is QuoteBlock quoteBlock)
            {
                return RenderQuoteBlock(quoteBlock);
            }
            else if (block is FencedCodeBlock fencedCodeBlock)
            {
                return RenderFencedCodeBlock(fencedCodeBlock);
            }
            else if (block is CodeBlock codeBlock)
            {
                return RenderCodeBlock(codeBlock);
            }
            else if (block is HtmlBlock htmlBlock)
            {
                return RenderHtmlBlock(htmlBlock);
            }
            else if (block is ThematicBreakBlock thematicBreakBlock)
            {
                return RenderThematicBreakBlock(thematicBreakBlock);
            }
            else if (block is ListBlock listBlock)
            {
                return RenderListBlock(listBlock);
            }
            else if (block is Table table)
            {
                return RenderTable(table);
            }
            else if (block is ContainerBlock containerBlock)
            {
                return RenderContainerBlock(containerBlock);
            }
            else
            {
                return new TextBlock();
            }
        }

        private FrameworkElement RenderContainerBlock(ContainerBlock containerBlock)
        {
            StackPanel documentElement = new StackPanel()
            {
                Margin = new Thickness(0, 0, 0, NormalSize)
            };

            foreach (var renderedBlock in RenderBlocks(containerBlock))
                documentElement.Children.Add(renderedBlock);

            return documentElement;
        }

        public FrameworkElement RenderTable(Table table)
        {
            Border tableElement = new Border()
            {
                BorderThickness = new Thickness(0, 0, 1, 1),
                Margin = new Thickness(0, 0, 0, NormalSize)
            };

            Grid tableContentElement = new Grid();

            tableElement.Child = tableContentElement;
            tableElement
                .BindTableBackground()
                .BindTableBorder();

            foreach (var col in table.ColumnDefinitions)
                tableContentElement.ColumnDefinitions.Add(
                    new ColumnDefinition()
                    {
                        Width = GridLength.Auto
                    });

            int rowIndex = 0;
            foreach (var block in table)
            {
                if (block is not TableRow row)
                    continue;

                tableContentElement.RowDefinitions.Add(
                    new RowDefinition()
                    {
                        Height = GridLength.Auto
                    });

                int colIndex = 0;
                foreach (var colBlock in row)
                {
                    if (colBlock is not TableCell cell)
                        continue;

                    Border cellElement = new Border()
                    {
                        BorderThickness = new Thickness(1, 1, 0, 0),
                        Padding = new Thickness(NormalSize / 2, NormalSize / 4, NormalSize / 2, NormalSize / 4)
                    };

                    FrameworkElement cellContentElement =
                        RenderBlock(cell);

                    cellElement.Child = cellContentElement;
                    cellElement
                        .BindTableBorder();

                    cellContentElement.Margin = new Thickness(0);

                    if (rowIndex % 2 == 1)
                        cellElement.BindTableStripe();

                    Grid.SetRow(cellElement, rowIndex);
                    Grid.SetColumn(cellElement, colIndex);

                    tableContentElement.Children.Add(cellElement);

                    colIndex++;
                }

                rowIndex++;
            }

            return tableElement;
        }

        public FrameworkElement RenderListBlock(ListBlock listBlock)
        {
            int itemCount = listBlock.Count;

            Func<int, string> markerTextGetter = listBlock.IsOrdered ?
                index => $"{index+1}." :
                index => "-";

            Border listElement = new Border()
            {
                Margin = new Thickness(NormalSize / 2, 0, 0, NormalSize)
            };

            Grid listContentElement = new Grid();

            listElement.Child =
                listContentElement;

            listContentElement.ColumnDefinitions.Add(
                new ColumnDefinition()
                {
                    Width = GridLength.Auto,
                });

            listContentElement.ColumnDefinitions.Add(
                new ColumnDefinition());

            for (int i = 0; i < itemCount; i++)
            {
                listContentElement.RowDefinitions.Add(
                    new RowDefinition()
                    {
                        Height = GridLength.Auto
                    });
            }

            int index = 0;
            FrameworkElement? lastRenderedItemBlock = null;
            foreach (var itemBlock in listBlock)
            {
                if (RenderBlock(itemBlock) is FrameworkElement renderedItemBlock)
                {
                    lastRenderedItemBlock = renderedItemBlock;
                    renderedItemBlock.Margin = renderedItemBlock.Margin with
                    {
                        Bottom = renderedItemBlock.Margin.Bottom / 4
                    };

                    TextBlock marker = new TextBlock();
                    Grid.SetRow(marker, index);
                    Grid.SetColumn(marker, 0);
                    marker.Text = markerTextGetter.Invoke(index);
                    marker.Margin = new Thickness(0, 0, NormalSize / 2, 0);

                    Grid.SetRow(renderedItemBlock, index);
                    Grid.SetColumn(renderedItemBlock, 1);

                    listContentElement.Children.Add(marker);
                    listContentElement.Children.Add(renderedItemBlock);

                    index++;
                }
            }

            if (lastRenderedItemBlock != null)
                lastRenderedItemBlock.Margin = lastRenderedItemBlock.Margin with
                {
                    Bottom = 0
                };

            return listElement;
        }

        public FrameworkElement RenderThematicBreakBlock(ThematicBreakBlock thematicBreakBlock)
        {
            Border thematicBreakElement = new Border()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Height = 1,
                Margin = new Thickness(0, 0, 0, NormalSize)
            };

            thematicBreakElement
                .BindThematicBreak();

            return thematicBreakElement;
        }

        public FrameworkElement RenderHtmlBlock(HtmlBlock htmlBlock)
        {
            return new TextBlock();
        }

        public FrameworkElement RenderFencedCodeBlock(FencedCodeBlock fencedCodeBlock)
        {
            if (string.IsNullOrWhiteSpace(fencedCodeBlock.Info))
                return RenderCodeBlock(fencedCodeBlock);

            Border codeElement = new Border()
            {
                CornerRadius = new CornerRadius(3),
                Margin = new Thickness(0, 0, 0, NormalSize)
            };

            TextBlock codeContentElement = new TextBlock()
            {
                TextWrapping = TextWrapping.Wrap,
                Padding = new Thickness(NormalSize / 2),

                FontSize = NormalSize,
                FontFamily = GetCodeTextFontFamily(),
            };

            codeElement.Child =
                codeContentElement;
            codeElement
                .BindCodeBlockBackground()
                .BindCodeBlockBorder();

            codeContentElement
                .BindCodeBlockForeground();

            if (fencedCodeBlock.Inline != null)
                codeContentElement.Inlines.AddRange(
                    RenderInlines(fencedCodeBlock.Inline));

            var language = ColorCode.Languages.FindById(fencedCodeBlock.Info);

            StyleDictionary styleDict = ColorMode switch
            {
                ColorMode.Light => StyleDictionary.DefaultLight,
                ColorMode.Dark => StyleDictionary.DefaultDark,

                _ => StyleDictionary.DefaultDark
            };

            WpfSyntaxHighLighting writer = new WpfSyntaxHighLighting(ColorCode.Styling.StyleDictionary.DefaultDark);
            writer.FormatTextBlock(fencedCodeBlock.Lines.ToString(), language, codeContentElement);


            return codeElement;
        }

        public FrameworkElement RenderCodeBlock(CodeBlock codeBlock)
        {
            Border codeElement = new Border()
            {
                CornerRadius = new CornerRadius(3),
                Margin = new Thickness(0, 0, 0, NormalSize)
            };

            TextBlock codeContentElement = new TextBlock()
            {
                TextWrapping = TextWrapping.Wrap,
                Padding = new Thickness(NormalSize / 2),

                FontSize = NormalSize,
                FontFamily = GetCodeTextFontFamily(),
            };

            codeElement.Child =
                codeContentElement;
            codeElement
                .BindCodeBlockBackground()
                .BindCodeBlockBorder();

            codeContentElement
                .BindCodeBlockForeground();

            if (codeBlock.Inline != null)
                codeContentElement.Inlines.AddRange(
                    RenderInlines(codeBlock.Inline));

            codeContentElement.Inlines.Add(
                new WpfDocs.Run(codeBlock.Lines.ToString()));

            return codeElement;
        }

        public FrameworkElement RenderQuoteBlock(QuoteBlock quoteBlock)
        {
            Border quoteElement = new Border()
            {
                BorderThickness = new Thickness(NormalSize / 3, 0, 0, 0),
                CornerRadius = new CornerRadius(NormalSize / 4),
                Padding = new Thickness(NormalSize / 2, 0, 0, 0),
                Margin = new Thickness(0, 0, 0, NormalSize),
            };

            StackPanel quoteContentPanel = new StackPanel();

            quoteElement.Child =
                quoteContentPanel;
            quoteElement
                .BindQuoteBlockBackground()
                .BindQuoteBlockBorder();

            foreach (var renderedBlock in RenderBlocks(quoteBlock))
                quoteContentPanel.Children.Add(renderedBlock);

            return quoteElement;
        }

        public FrameworkElement RenderHeadingBlock(HeadingBlock headingBlock)
        {
            double fontSize = headingBlock.Level switch
            {
                1 => Heading1Size,
                2 => Heading2Size,
                3 => Heading3Size,
                4 => Heading4Size,
                _ => NormalSize 
            };

            TextBlock headingElement = new TextBlock()
            {
                FontSize = fontSize,
                FontWeight = FontWeights.Medium,
                Margin = new Thickness(0, 0, 0, NormalSize)
            };

            headingElement
                .BindMainForeground()
                .BindMainBackground();

            if (headingBlock.Inline != null)
                headingElement.Inlines.AddRange(
                    RenderInlines(headingBlock.Inline));

            return headingElement;
        }

        public FrameworkElement RenderParagraphBlock(ParagraphBlock paragraphBlock)
        {
            TextBlock paragraphElement = new TextBlock()
            {
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, NormalSize),

                FontSize = NormalSize,
            };

            paragraphElement
                .BindMainForeground()
                .BindMainBackground();

            if (paragraphBlock.Inline != null)
                paragraphElement.Inlines.AddRange(
                    RenderInlines(paragraphBlock.Inline));

            return paragraphElement;
        }






        public List<WpfDocs.Inline> RenderInlines(IEnumerable<Inline> inlines)
        {
            List<WpfDocs.Inline> inlineElements = new List<WpfDocs.Inline>();

            foreach (var inline in inlines)
                if (RenderInline(inline) is WpfDocs.Inline wpfInline)
                    inlineElements.Add(wpfInline);

            return inlineElements;
        }

        public WpfDocs.Inline RenderInline(Inline inline)
        {
            if (inline is LiteralInline literalInline)
            {
                return RenderLiteralInline(literalInline);
            }
            else if (inline is LinkInline linkInline)
            {
                return RenderLinkInline(linkInline);
            }
            else if (inline is LineBreakInline lineBreakInline)
            {
                return RenderLineBreakInline(lineBreakInline);
            }
            else if (inline is HtmlInline htmlInline)
            {
                return RenderHtmlInline(htmlInline);
            }
            else if (inline is HtmlEntityInline htmlEntityInline)
            {
                return RenderHtmlEntityInline(htmlEntityInline);
            }
            else if (inline is EmphasisInline emphasisInline)
            {
                return RenderEmphasisInline(emphasisInline);
            }
            else if (inline is CodeInline codeInline)
            {
                return RenderCodeInline(codeInline);
            }
            else if (inline is AutolinkInline autolinkInline)
            {
                return RenderAutolinkInline(autolinkInline);
            }
            else if (inline is DelimiterInline delimiterInline)
            {
                return RenderDelimiterInline(delimiterInline);
            }
            else if (inline is ContainerInline containerInline)
            {
                return RenderContainerInline(containerInline);
            }
            else if (inline is TaskList taskListInline)
            {
                return RenderTaskListInline(taskListInline);
            }
            else
            {
                return new WpfDocs.Run();
            }
        }

        private WpfDocs.Inline RenderTaskListInline(TaskList taskListInline)
        {
            return new CheckBox()
            {
                IsChecked = taskListInline.Checked,
                IsEnabled = false,
            }.WrapWithContainer();
        }

        private WpfDocs.Inline RenderAutolinkInline(AutolinkInline autolinkInline)
        {
            return new WpfDocs.Run(autolinkInline.Url);
        }

        private WpfDocs.Inline RenderCodeInline(CodeInline codeInline)
        {
            Border border = new Border()
            {
                CornerRadius = new CornerRadius(3),
                Padding = new Thickness(NormalSize / 6, 0, NormalSize / 6, 0),
                Margin = new Thickness(NormalSize / 6, 0, NormalSize / 6, 0)
            };

            TextBlock textBlock = new TextBlock();

            border.Child = textBlock;

            border
                .BindCodeInlineBackground()
                .BindCodeInlineBorder();

            textBlock.Text = codeInline.Content;

            return border.WrapWithContainer();
        }

        private WpfDocs.Inline RenderContainerInline(ContainerInline containerInline)
        {
            WpfDocs.Span span = new WpfDocs.Span();

            span.Inlines.AddRange(
                RenderInlines(containerInline));

            return span;
        }

        private WpfDocs.Inline RenderEmphasisInline(EmphasisInline emphasisInline)
        {
            WpfDocs.Span span = new WpfDocs.Span();

            switch (emphasisInline.DelimiterChar)
            {
                case '*' when emphasisInline.DelimiterCount == 2: // bold
                case '_' when emphasisInline.DelimiterCount == 2: // bold
                    span.FontWeight = FontWeights.Bold;
                    break;
                case '*': // italic
                case '_': // italic
                    span.FontStyle = FontStyles.Italic;
                    break;
                case '~': // 2x strike through, 1x subscript
                    if (emphasisInline.DelimiterCount == 2)
                        span.TextDecorations.Add(TextDecorations.Strikethrough);
                    else
                        WpfDocs.Typography.SetVariants(span, FontVariants.Subscript);
                    break;
                case '^': // 1x superscript
                    WpfDocs.Typography.SetVariants(span, FontVariants.Subscript);
                    break;
                case '+': // 2x underline
                    span.TextDecorations.Add(TextDecorations.Underline);
                    break;
                case '=': // 2x Marked
                    span.SetResourceReference(WpfDocs.Span.BackgroundProperty, MarkdownResKey.Mark);
                    break;
            }

            span.Inlines.AddRange(
                RenderInlines(emphasisInline));

            return span;
        }

        private WpfDocs.Inline RenderHtmlEntityInline(HtmlEntityInline htmlEntityInline)
        {
            return new WpfDocs.Run(htmlEntityInline.Transcoded.ToString());
        }

        private WpfDocs.Inline RenderHtmlInline(HtmlInline htmlInline)
        {
            return new WpfDocs.Run();
        }

        private WpfDocs.Inline RenderLineBreakInline(LineBreakInline lineBreakInline)
        {
            return new WpfDocs.Run("\n");
        }

        private WpfDocs.Inline RenderDelimiterInline(DelimiterInline delimiterInline)
        {
            return new WpfDocs.Run(delimiterInline.ToLiteral());
        }

        private WpfDocs.Inline RenderLinkInline(LinkInline linkInline)
        {

            Uri? uri = null;

            if (linkInline.Url != null && Uri.TryCreate(linkInline.Url, UriKind.RelativeOrAbsolute, out Uri? _uri))
                uri = _uri;

            if (linkInline.IsImage)
            {
                Image img = new Image();

                if (uri != null)
                    img.Source = new BitmapImage(uri);

                return img.WrapWithContainer();
            }
            else
            {
                WpfDocs.Hyperlink link = new WpfDocs.Hyperlink();

                WpfDocs.Inline? linkContent = null;

                if (linkInline.Label != null)
                    linkContent = new WpfDocs.Run(linkInline.Label);

                if (linkContent != null)
                    link.Inlines.Add(linkContent);
                if (RenderContainerInline(linkInline) is WpfDocs.Inline extraInline)
                    link.Inlines.Add(extraInline);

                link.Click += (s, e) =>
                {
                    LinkNavigate?.Invoke(linkInline, new MarkdownLinkNavigateEventArgs(linkInline.Url));
                };

                return link;
            }
        }

        private WpfDocs.Inline RenderLiteralInline(LiteralInline literalInline)
        {
            return new WpfDocs.Run(literalInline.Content.ToString());
        }









        private FontFamily GetNormalTextFontFamily()
        {
            return new FontFamily("-apple-system,BlinkMacSystemFont,Segoe UI Adjusted,Segoe UI,Liberation Sans,sans-serif");
        }

        private FontFamily GetCodeTextFontFamily()
        {
            return new FontFamily("ui-monospace,Cascadia Code,Segoe UI Mono,Liberation Mono,Menlo,Monaco,Consolas,monospace");
        }
    }
}
