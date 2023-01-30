using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using HackerNews.Converters;
using HackerNews.Model;

namespace HackerNews.ViewModels;

public partial class MainViewViewModel : ViewModelBase, ILazyLoadable
{
    [ObservableProperty] private bool _isVisible;
    [ObservableProperty] private INavigationService? _navigation;
    [ObservableProperty] private bool _isMenuOpen;
    [ObservableProperty] private ObservableCollection<ItemsViewModel>? _feeds;
    [ObservableProperty] private ItemsViewModel? _currentFeed;
    [ObservableProperty] private DateTimeOffset _lastUpdated;
    [ObservableProperty] private string? _lastUpdatedAgo;

    public MainViewViewModel()
    {
        _navigation = Ioc.Default.GetService<INavigationService>();

        _feeds = new ObservableCollection<ItemsViewModel>
        {
            new ("topstories", "Top Stories", "ArrowTrendingFilledIcon", OpenFeedAsync),
            // TODO: Catch Up
            new ("newstories", "New Stories", "BookExclamationMarkFilledIcon", OpenFeedAsync),
            new ("beststories", "Best Stories", "ThumbLikeFilledIcon", OpenFeedAsync),
            new ("askstories", "Ask HN", "BookQuestionMarkFilledIcon", OpenFeedAsync),
            new ("showstories", "Show HN", "CheckmarkFilledIcon", OpenFeedAsync),
            new ("jobstories", "Jobs", "BriefcaseFilledIcon", OpenFeedAsync),
            // TODO: Saved Stories
        };

        _currentFeed = _feeds.FirstOrDefault();

        NavigationCommand = new AsyncRelayCommand(ToggleMenuAsync);

        LoadCommand = new AsyncRelayCommand(LoadAsync);

        SearchCommand = new AsyncRelayCommand(SearchAsync);

        RunUpdateTimer();
    }

    public IAsyncRelayCommand LoadCommand { get; }

    public IAsyncRelayCommand NavigationCommand { get; }

    public IAsyncRelayCommand SearchCommand { get; }

    public bool IsLoaded()
    {
        // TODO:
        return false;
    }

    public async Task LoadAsync()
    {
        if (CurrentFeed is { })
        {
            LastUpdated = DateTimeOffset.Now;

            UpdateLastUpdatedAgo();

            await CurrentFeed.LoadAsync();

            var navigation = Ioc.Default.GetService<INavigationService>();
            if (navigation is { })
            {
                await navigation.Clear();
                await navigation.NavigateAsync(CurrentFeed);
            }
        }
    }

    public async Task UpdateAsync()
    {
        if (CurrentFeed is { })
        {
            await CurrentFeed.UpdateAsync();
        }
    }

    public async Task<bool> BackAsync()
    {
        var navigation = Ioc.Default.GetService<INavigationService>();
        if (navigation is { })
        {
            return await navigation.BackAsync();
        }

        return await Task.FromResult(false);
    }

    private async Task SearchAsync()
    {
        // TODO:
        await Task.Yield();
    }

    private async Task OpenFeedAsync(ItemsViewModel feed)
    {
        CurrentFeed = feed;

        IsMenuOpen = false;
        
        await LoadAsync();
    }

    private async Task ToggleMenuAsync()
    {
        IsMenuOpen = !IsMenuOpen;
        await Task.Yield();
    }

    private void RunUpdateTimer()
    {
        DispatcherTimer.Run(
            () =>
            {
                UpdateLastUpdatedAgo();
                return true;
            },
            TimeSpan.FromMinutes(1));
    }

    private void UpdateLastUpdatedAgo()
    {
        LastUpdatedAgo = $"Last updated {StringConverter.ToTimeAgoString(LastUpdated)} ago";
    }
}
