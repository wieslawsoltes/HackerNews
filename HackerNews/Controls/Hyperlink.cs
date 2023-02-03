using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Input;
using Avalonia.Media;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using HackerNews.Model;

namespace HackerNews.Controls;

public class Hyperlink : InlineUIContainer
{
    private readonly Underline _underline;

    public Span Content => _underline;

    public Hyperlink(string? href)
    {
        _underline = new Underline();

        var textBlock = new TextBlock
        {
            Inlines = new InlineCollection
            {
                _underline
            }
        };

        var button = new Button
        {
            Background = Brushes.Transparent,
            Margin = new Thickness(),
            Padding = new Thickness(),
            Cursor = new Cursor(StandardCursorType.Hand),
            Content = textBlock
        };

        if (href is { })
        {
            var url = new Uri(href);

            button.Command =  new AsyncRelayCommand(async () => await OpenUrl(url));

            ToolTip.SetTip(button, href);
        }

        Child = button;
    }

    private static async Task OpenUrl(Uri? url)
    {
        if (url is { })
        {
            var browser = Ioc.Default.GetService<IBrowserService>();
            if (browser is { })
            {
                await browser.OpenBrowserAsync(url);
            }
        }
    }
}
