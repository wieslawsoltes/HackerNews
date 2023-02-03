using System;
using System.Globalization;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Media;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using HackerNews.Controls;
using HackerNews.Model;
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
            case AnchorNode anchorNode:
            {
                var hyperlink = new Hyperlink(anchorNode.Href);
                var content = hyperlink.Content;
                content.Classes.Add("pre");
                content.Foreground = Brushes.Red;
                inlines.Add(hyperlink);
                childInlines = content.Inlines;
                break;
            }
            case PreNode _:
            {
                // TODO: Disable text wrapping by using custom Inline element.
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
