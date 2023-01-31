using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls.Documents;
using Avalonia.Data.Converters;
using Avalonia.Media;
using HackerNews.Model.Html;

namespace HackerNews.Converters;

public class InlinesConverter : IValueConverter
{
    public static InlinesConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Node node)
        {
            var inlines = new InlineCollection();

            PrintNode(node, inlines);

            return inlines;
        }

        return AvaloniaProperty.UnsetValue;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private static void PrintNode(Node node, InlineCollection inlines)
    {
        InlineCollection? childInlines = null;

        switch (node)
        {
            case TextNode textNode:
            {
                if (textNode.Text is { })
                {
                    var run = new Run
                    {
                        Text = textNode.Text
                    };
                    run.Classes.Add("text");
                    inlines.Add(run);
                }
                break;
            }
            // TODO: Add line break after italic.
            // case QuoteNode quoteNode:
            // {
            //     var italic = new Italic();
            //     inlines.Add(italic);
            //     childInlines = italic.Inlines;
            //     break;
            // }
            case ItalicNode _:
            {
                var italic = new Italic();
                italic.Classes.Add("i");
                inlines.Add(italic);
                childInlines = italic.Inlines;
                break;
            }
            case ParagraphNode _:
            {
                var span = new Span();
                span.Classes.Add("p");
                inlines.Add(new LineBreak());
                inlines.Add(new LineBreak());
                inlines.Add(span);
                childInlines = span.Inlines;
                break;
            }
            case AnchorNode _:
            {
                // TODO: Use Hyperlink inline.
                var underline = new Underline();
                underline.Classes.Add("a");
                underline.Foreground = Brushes.Red;
                inlines.Add(underline);
                childInlines = underline.Inlines;
                break;
            }
            case PreNode _:
            {
                // TODO: Disable text wrapping by using custom Inline element.
                // TODO: Set font to monospaced.
                var span = new Span();
                span.Classes.Add("pre");
                inlines.Add(new LineBreak());
                inlines.Add(new LineBreak());
                inlines.Add(span);
                childInlines = span.Inlines;
                break;
                
            }
            case CodeNode _:
            {
                // TODO: Set font to monospaced.
                var span = new Span();
                span.Classes.Add("code");
                inlines.Add(span);
                childInlines = span.Inlines;
                break;
            }
        }

        foreach (var childNode in node.Nodes)
        {
            PrintNode(childNode, childInlines ?? inlines);
        }
    }
}
