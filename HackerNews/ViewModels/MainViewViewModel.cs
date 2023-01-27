﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using HackerNews.Model;

namespace HackerNews.ViewModels;

public partial class MainViewViewModel : ViewModelBase, ILazyLoadable
{
    private readonly IHackerNewsApi? _api;

    [ObservableProperty] private INavigation? _navigation;
    [ObservableProperty] private ObservableCollection<ItemsViewModel>? _feeds;
    [ObservableProperty] private ItemsViewModel? _currentFeed;
    [ObservableProperty] private DateTimeOffset _lastUpdated;
    [ObservableProperty] private string? _lastUpdatedAgo;

    public MainViewViewModel()
    {
        _api = Ioc.Default.GetService<IHackerNewsApi>();
        _navigation = Ioc.Default.GetService<INavigation>();

        _feeds = new ObservableCollection<ItemsViewModel>
        {
            new ("topstories", "Top Stories"),
            new ("newstories", "New Stories"),
            new ("beststories", "Best Stories"),
            new ("askstories", "Ask HN"),
            new ("showstories", "Show HN"),
            new ("jobstories", "Jobs"),
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
        if (CurrentFeed is { })
        {
            LastUpdated = DateTimeOffset.Now;

            UpdateLastUpdatedAgo();

            await CurrentFeed.LoadAsync();

            if (Navigation is { })
            {
                await Navigation.Clear();
                await Navigation.NavigateAsync(CurrentFeed);
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
        if (Navigation is { })
        {
            return await Navigation.BackAsync();
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
