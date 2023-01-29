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

public partial class App : Application
{
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

    private static void ConfigureServices()
    {
        Ioc.Default.ConfigureServices(
            new ServiceCollection()
                // Services
                .AddSingleton<IHackerNewsService, HackerNewsServiceV0>()
                // ViewModels
                .AddSingleton<INavigationService, NavigationViewModel>()
                .AddSingleton<IStateManager, StateManagerViewModel>()
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
}
