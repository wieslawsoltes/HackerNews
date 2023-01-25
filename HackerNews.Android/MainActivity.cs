using Android.App;
using Android.Content.PM;
using Avalonia.Android;

namespace HackerNews.Android;

[Activity(Label = "HackerNews", Theme = "@style/MyTheme.NoActionBar", Icon = "@drawable/icon", LaunchMode = LaunchMode.SingleInstance, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
public class MainActivity : AvaloniaMainActivity
{
}
