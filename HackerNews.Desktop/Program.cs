using Avalonia;
using System;

namespace HackerNews.Desktop;

class Program
{
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .AfterSetup(_ =>
        {
            App.BrowserService = new DesktopBrowserService();
            App.ShareService = new DesktopShareService();
        })
        .StartWithClassicDesktopLifetime(args);

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();
}
