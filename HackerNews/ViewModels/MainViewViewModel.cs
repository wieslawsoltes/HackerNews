using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HackerNews.Model;
using HackerNews.Services;

namespace HackerNews.ViewModels;

public partial class MainViewViewModel : ViewModelBase, ILazyLoadable
{
    private readonly IHackerNewsApi? _api;

    [ObservableProperty] private NavigationViewModel? _navigation;
    [ObservableProperty] private ObservableCollection<ItemsViewModel>? _feeds;
    [ObservableProperty] private ItemsViewModel? _currentFeed;
    [ObservableProperty] private DateTimeOffset _lastUpdated;
    [ObservableProperty] private string? _lastUpdatedAgo;

    public MainViewViewModel()
    {
        _api = new HackerNewsApiV0();
        _navigation = new NavigationViewModel();

        _feeds = new ObservableCollection<ItemsViewModel>
        {
            new (_api, _navigation, "topstories", "Top Stories"),
            new (_api, _navigation, "newstories", "New Stories"),
            new (_api, _navigation, "beststories", "Best Stories"),
            new (_api, _navigation, "askstories", "Ask HN"),
            new (_api, _navigation, "showstories", "Show HN"),
            new (_api, _navigation, "jobstories", "Jobs"),
        };

        _currentFeed = _feeds.FirstOrDefault();

        NavigationCommand = new AsyncRelayCommand(BackAsync);

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
        if (_currentFeed is { })
        {
            LastUpdated = DateTimeOffset.Now;

            UpdateLastUpdatedAgo();

            await _currentFeed.LoadAsync();

            if (_navigation is { })
            {
                await _navigation.Clear();
                await _navigation.NavigateAsync(_currentFeed);
            }
        }
    }

    public async Task UpdateAsync()
    {
        if (_currentFeed is { })
        {
            await _currentFeed.UpdateAsync();
        }
    }

    public async Task<bool> BackAsync()
    {
        if (_navigation is { })
        {
            return await _navigation.BackAsync();
        }

        // TODO:
        return await Task.FromResult(false);
    }

    public async Task SearchAsync()
    {
        // TODO:
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
        LastUpdatedAgo = $"Last updated {ItemViewModel.ToTimeAgoString(LastUpdated)} ago";
    }
}
