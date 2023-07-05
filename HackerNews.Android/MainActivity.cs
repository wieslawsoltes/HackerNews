using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Avalonia;
using Avalonia.Android;
using HackerNews.Model;
using Xamarin.Essentials;
namespace HackerNews.Android;

[Activity(Label = "HackerNews", Theme = "@style/MyTheme.NoActionBar", Icon = "@drawable/icon", MainLauncher = true, LaunchMode = LaunchMode.SingleInstance, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>, IBrowserService, IShareService
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

    public async Task ShareTextAsync(string title, string text, string uri)
    {
        await Share.RequestAsync(new ShareTextRequest
        {
            Title = title,
            Text = text,
            Subject = text,
            Uri = uri
        });
    }

    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        return base.CustomizeAppBuilder(builder)
            .WithInterFont()
            .AfterSetup(_ =>
            {
                App.BrowserService = this;
                App.ShareService = this;
            });
    }
}
