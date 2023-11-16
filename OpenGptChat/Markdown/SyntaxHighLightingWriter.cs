using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using ColorCode;
using ColorCode.Common;
using ColorCode.Parsing;
using ColorCode.Styling;
using Wpf = System.Windows;
using WpfDocs = System.Windows.Documents;

namespace OpenGptChat.Markdown
{
    public class WpfSyntaxHighLighting : CodeColorizerBase
    {
        private readonly static BrushConverter BrushConverter = new BrushConverter();

        private WpfDocs.InlineCollection? InlineCollection { get; set; }

        public WpfSyntaxHighLighting(StyleDictionary? Styles = null, ILanguageParser? languageParser = null) : base(Styles, languageParser)
        {

        }

        public void FormatTextBlock(string sourceCode, ILanguage? language, TextBlock textBlock)
        {
            FormatInlines(sourceCode, language, textBlock.Inlines);
        }

        public void FormatInlines(string sourceCode, ILanguage? language, WpfDocs.InlineCollection inlines)
        {
            this.InlineCollection = inlines;

            if (language != null)
            {
                languageParser.Parse(sourceCode, language, Write);
            }
            else
            {
                CreateSpan(sourceCode, null);
            }
        }

        protected override void Write(string parsedSourceCode, IList<Scope> scopes)
        {
            var styleInsertions = new List<TextInsertion>();

            foreach (Scope scope in scopes)
                GetStyleInsertionsForCapturedStyle(scope, styleInsertions);

            styleInsertions.SortStable((x, y) => x.Index.CompareTo(y.Index));

            int offset = 0;

            Scope? previousScope = null;

            foreach (var styleinsertion in styleInsertions)
            {
                var text = parsedSourceCode.Substring(offset, styleinsertion.Index - offset);
                CreateSpan(text, previousScope);
                if (!string.IsNullOrWhiteSpace(styleinsertion.Text))
                {
                    CreateSpan(text, previousScope);
                }
                offset = styleinsertion.Index;

                previousScope = styleinsertion.Scope;
            }

            var remaining = parsedSourceCode.Substring(offset);
            // Ensures that those loose carriages don't run away!
            if (remaining != "\r")
            {
                CreateSpan(remaining, null);
            }
        }

        private void CreateSpan(string text, Scope? scope)
        {
            var span = new Span();
            var run = new Run
            {
                Text = text
            };

            // Styles and writes the text to the span.
            if (scope != null)
                StyleRun(run, scope);
            span.Inlines.Add(run);

            InlineCollection?.Add(span);
        }

        private void StyleRun(Run run, Scope scope)
        {
            string? foreground = null;
            string? background = null;
            bool italic = false;
            bool bold = false;

            if (Styles.Contains(scope.Name))
            {
                ColorCode.Styling.Style style = Styles[scope.Name];

                foreground = style.Foreground;
                background = style.Background;
                italic = style.Italic;
                bold = style.Bold;
            }

            if (!string.IsNullOrWhiteSpace(foreground))
            {
                try
                {
                    run.Foreground = BrushConverter.ConvertFromString(foreground) as Brush;
                }
                catch { }
            }

            //Background isn't supported, but a workaround could be created.

            if (italic)
                run.FontStyle = FontStyles.Italic;

            if (bold)
                run.FontWeight = FontWeights.Bold;
        }

        private void GetStyleInsertionsForCapturedStyle(Scope scope, ICollection<TextInsertion> styleInsertions)
        {
            styleInsertions.Add(new TextInsertion
            {
                Index = scope.Index,
                Scope = scope
            });

            foreach (Scope childScope in scope.Children)
                GetStyleInsertionsForCapturedStyle(childScope, styleInsertions);

            styleInsertions.Add(new TextInsertion
            {
                Index = scope.Index + scope.Length
            });
        }
    }
}
