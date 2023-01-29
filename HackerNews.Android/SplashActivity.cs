using Android.App;
using Android.Content;
using Android.Net;
using Avalonia.Android;
using HackerNews.Model;

namespace HackerNews.Android;

[Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
public class SplashActivity : AvaloniaSplashActivity<App>, IBrowserService
{
    public void OpenUrl(System.Uri url)
    {
        var uri = Uri.Parse (url.ToString());
        var intent = new Intent (Intent.ActionView, uri);
        StartActivity (intent);
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
