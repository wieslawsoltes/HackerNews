using System;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HackerNews.Model;
using HackerNews.Services;

namespace HackerNews.ViewModels;

public partial class MainViewViewModel : ViewModelBase, ILazyLoadable
{
    private readonly HackerNewsApiV0? _api;

    [ObservableProperty] private NavigationViewModel? _navigation;
    [ObservableProperty] private ItemsViewModel? _currentItems;
    [ObservableProperty] private DateTimeOffset _lastUpdated;
    [ObservableProperty] private string? _lastUpdatedAgo;

    public MainViewViewModel()
    {
        _api = new HackerNewsApiV0();
        _navigation = new NavigationViewModel();

        _currentItems = new ItemsViewModel(_api, _navigation, HackerNewsApiV0.TopStories, "Top Stories");

        NavigationCommand = new AsyncRelayCommand(BackAsync);

        LoadCommand = new AsyncRelayCommand(LoadAsync);

        SearchCommand = new AsyncRelayCommand(SearchAsync);

        DispatcherTimer.Run(() =>
            {
                UpdateLastUpdatedAgo();
                return true;
            },
            TimeSpan.FromMinutes(1));
    }

    public IAsyncRelayCommand LoadCommand { get; }

    public IAsyncRelayCommand NavigationCommand { get; }

    public IAsyncRelayCommand SearchCommand { get; }

    public bool IsLoaded()
    {
        // TODO:
        return true;
    }

    public async Task LoadAsync()
    {
        if (_currentItems is { })
        {
            LastUpdated = DateTimeOffset.Now;

            UpdateLastUpdatedAgo();

            await _currentItems.LoadAsync();

            if (_navigation is { })
            {
                await _navigation.NavigateAsync(_currentItems);
            }
        }
    }

    public async Task UpdateAsync()
    {
        if (_currentItems is { })
        {
            await _currentItems.UpdateAsync();
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

    private void UpdateLastUpdatedAgo()
    {
        LastUpdatedAgo = $"Last updated {ItemViewModel.ToTimeAgoString(LastUpdated)} ago";
    }
}
