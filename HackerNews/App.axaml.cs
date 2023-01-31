using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.DependencyInjection;
using HackerNews.Model;
using HackerNews.Services;
using HackerNews.ViewModels;
using HackerNews.Views;
using Microsoft.Extensions.DependencyInjection;

namespace HackerNews;

public partial class App : Application, IBrowserService, IShareService
{
    public static IBrowserService? BrowserService { get; set; }

    public static IShareService? ShareService { get; set; }

    public App()
    {
        ConfigureServices();
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = Ioc.Default.GetService<MainViewViewModel>()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime single)
        {
            single.MainView = new MainView
            {
                DataContext = Ioc.Default.GetService<MainViewViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void ConfigureServices()
    {
        Ioc.Default.ConfigureServices(
            new ServiceCollection()
                // Services
                .AddSingleton<ILog, ConsoleLog>()
                .AddSingleton<IHackerNewsService, HackerNewsServiceV0>()
                .AddSingleton<INavigationService, NavigationViewModel>()
                .AddSingleton<IStateStorageService, StateStorageViewModel>()
                .AddSingleton<IBrowserService, App>(_ => this)
                .AddSingleton<IShareService, App>(_ => this)
                // ViewModels
                .AddTransient<ItemViewModel>()
                .AddTransient<ItemsViewModel>()
                .AddTransient<CommentsViewModel>()
                .AddTransient<PollViewModel>()
                .AddTransient<UserViewModel>()
                .AddTransient<SubmittedViewModel>()
                .AddTransient<MainViewViewModel>()
                .BuildServiceProvider());
    }

    async Task IBrowserService.OpenBrowserAsync(System.Uri uri, bool external)
    {
        if (BrowserService is { })
        {
            await BrowserService.OpenBrowserAsync(uri, external);
        }
    }
    async Task IShareService.ShareTextAsync(string title, string text, string uri)
    {
        if (ShareService is { })
        {
            await ShareService.ShareTextAsync(title, text, uri);
        }
    }
}
