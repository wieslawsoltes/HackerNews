using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls.Documents;
using Avalonia.Data.Converters;
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
                    inlines.Add(textNode.Text);
                }
                break;
            }
            case QuoteNode quoteNode:
            {
                // TODO: Add line break after italic.
                var italic = new Italic();
                inlines.Add(italic);
                childInlines = italic.Inlines;
                break;
            }
            case ItalicNode italicNode:
            {
                var italic = new Italic();
                inlines.Add(italic);
                childInlines = italic.Inlines;
                break;
            }
            case ParagraphNode paragraphNode:
            {
                var span = new Span();
                inlines.Add(new LineBreak());
                inlines.Add(new LineBreak());
                inlines.Add(span);
                childInlines = span.Inlines;
                break;
            }
            case AnchorNode anchorNode:
            {
                // TODO: Use Hyperlink inline.
                var underline = new Underline();
                inlines.Add(underline);
                childInlines = underline.Inlines;
                break;
            }
            case PreNode preNode:
            {
                var span = new Span();
                inlines.Add(new LineBreak());
                inlines.Add(new LineBreak());
                inlines.Add(span);
                childInlines = span.Inlines;
                break;
            }
            case CodeNode codeNode:
            {
                var span = new Span();
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
