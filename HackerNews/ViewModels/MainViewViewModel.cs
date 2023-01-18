using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HackerNews.Model;
using HackerNews.Services;

namespace HackerNews.ViewModels;

public partial class MainViewViewModel : ViewModelBase, ILazyLoadable
{
    private readonly HackerNewsApiV0 _api;
    [ObservableProperty] private ItemsViewModel? _itemsView;

    public MainViewViewModel()
    {
        _api = new HackerNewsApiV0();
        _itemsView = new ItemsViewModel(_api, HackerNewsApiV0.TopStories, "Top Stories");

        LoadCommand = new AsyncRelayCommand(LoadAsync);
    }

    public IAsyncRelayCommand LoadCommand { get; }

    public bool IsLoaded()
    {
        // TODO:
        return true;
    }

    public async Task LoadAsync()
    {
        if (_itemsView is { })
        {
            await _itemsView.LoadAsync();
        }
    }

    public async Task UpdateAsync()
    {
        if (_itemsView is { })
        {
            await _itemsView.UpdateAsync();
        }
    }

    public async Task BackAsync()
    {
        if (_itemsView is { })
        {
            await _itemsView.BackAsync();
        }
    }
}
