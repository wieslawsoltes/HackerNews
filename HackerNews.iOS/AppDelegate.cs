using System.Threading.Tasks;
using Avalonia;
using Avalonia.iOS;
using Foundation;
using HackerNews.Model;
using Xamarin.Essentials;

namespace HackerNews.iOS;

// The UIApplicationDelegate for the application. This class is responsible for launching the 
// User Interface of the application, as well as listening (and optionally responding) to 
// application events from iOS.
[Register("AppDelegate")]
public class AppDelegate : AvaloniaAppDelegate<App>, IBrowserService
{
    public async Task OpenBrowserAsync(System.Uri uri, bool external = false)
    {
        try
        {
            await Browser.OpenAsync(
                uri, 
                external ? BrowserLaunchMode.External : BrowserLaunchMode.SystemPreferred);

            // TODO: External alternative.
            // var intent = new Intent (Intent.ActionView, Uri.Parse (uri.ToString()));
            // StartActivity (intent);
        }
        catch (System.Exception)
        {
            // ignored
        }
    }

    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        return base.CustomizeAppBuilder(builder)
            .AfterSetup(_ =>
            {
                App.BrowserService = this;
            });
    }
}
