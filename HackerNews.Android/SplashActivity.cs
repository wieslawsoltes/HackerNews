using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Net;
using Avalonia.Android;
using HackerNews.Model;
using Xamarin.Essentials;

namespace HackerNews.Android;

[Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
public class SplashActivity : AvaloniaSplashActivity<App>, IBrowserService
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

    protected override Avalonia.AppBuilder CustomizeAppBuilder(Avalonia.AppBuilder builder)
    {
        return base.CustomizeAppBuilder(builder)
            .AfterSetup(_ =>
            {
                App.BrowserService = this;
            });
    }

    protected override void OnResume()
    {
        base.OnResume();

        StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        Finish();
    }
}
