using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HackerNews.Model;
using HackerNews.Services;

namespace HackerNews.ViewModels;

public partial class MainViewViewModel : ViewModelBase, ILazyLoadable
{
    private readonly HackerNewsApiV0 _api;
    [ObservableProperty] private ItemsViewModel? _currentItems;

    public MainViewViewModel()
    {
        _api = new HackerNewsApiV0();
        _currentItems = new ItemsViewModel(_api, HackerNewsApiV0.TopStories, "Top Stories");

        LoadCommand = new AsyncRelayCommand(LoadAsync);

        SearchCommand = new AsyncRelayCommand(SearchAsync);
    }

    public IAsyncRelayCommand LoadCommand { get; }

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
        if (_currentItems is { })
        {
            await _currentItems.BackAsync();
        }
    }

    public async Task SearchAsync()
    {
        // TODO:
        await Task.Yield();
    }
}
