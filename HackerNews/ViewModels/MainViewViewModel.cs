using System.Threading.Tasks;
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

    public MainViewViewModel()
    {
        _api = new HackerNewsApiV0();
        _navigation = new NavigationViewModel();

        _currentItems = new ItemsViewModel(_api, _navigation, HackerNewsApiV0.TopStories, "Top Stories");

        NavigationCommand = new AsyncRelayCommand(BackAsync);

        LoadCommand = new AsyncRelayCommand(LoadAsync);

        SearchCommand = new AsyncRelayCommand(SearchAsync);
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

    public async Task BackAsync()
    {
        if (_navigation is { })
        {
            await _navigation.BackAsync();
        }
    }

    public async Task SearchAsync()
    {
        // TODO:
        await Task.Yield();
    }
}
