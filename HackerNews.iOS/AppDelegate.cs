using System.Threading.Tasks;
using Avalonia;
using Avalonia.iOS;
using Foundation;
using HackerNews.Model;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.ApplicationModel.DataTransfer;

namespace HackerNews.iOS;

// The UIApplicationDelegate for the application. This class is responsible for launching the 
// User Interface of the application, as well as listening (and optionally responding) to 
// application events from iOS.
[Register("AppDelegate")]
public class AppDelegate : AvaloniaAppDelegate<App>, IBrowserService, IShareService
{
    public async Task OpenBrowserAsync(System.Uri uri, bool external = false)
    {
        try
        {
            await Browser.OpenAsync(
                uri, 
                external ? BrowserLaunchMode.External : BrowserLaunchMode.SystemPreferred);
        }
        catch (System.Exception exception)
        {
            System.Console.WriteLine(exception);
        }
    }

    public async Task ShareTextAsync(string title, string text, string uri)
    {
        try
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Title = title,
                Text = text,
                Subject = text,
                Uri = uri
            });
        }
        catch (System.Exception exception)
        {
            System.Console.WriteLine(exception);
        }
    }

    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        return base.CustomizeAppBuilder(builder)
            .AfterSetup(_ =>
            {
                App.BrowserService = this;
                App.ShareService = this;
            });
    }
}
