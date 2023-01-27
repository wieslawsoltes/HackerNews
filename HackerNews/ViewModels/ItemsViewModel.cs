using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using HackerNews.Model;
using HackerNews.Services;

namespace HackerNews.ViewModels;

public partial class ItemsViewModel : ViewModelBase, ILazyLoadable
{
    private readonly IHackerNewsApi? _api;
    private readonly INavigation? _navigation;

    private readonly string? _storiesFeed;
    private List<int>? _ids;

    [ObservableProperty] private string? _title;
    [ObservableProperty] private ObservableCollection<ItemViewModel>? _items;

    public ItemsViewModel()
    {
    }

    public ItemsViewModel(IHackerNewsApi api, INavigation navigation, string storiesFeed, string title)
    {
        _api = api;
        _navigation = navigation;
        _storiesFeed = storiesFeed;
        _title = title;
    }

    public bool IsLoaded()
    {
        // TODO:
        return false;
    }

    public async Task LoadAsync()
    {
        if (_api is { } && _navigation is { } && _storiesFeed is { })
        {
            _ids ??= new List<int>();
            _ids.Clear();

            Items ??= new ObservableCollection<ItemViewModel>();
            Items.Clear();

            var json = await _api.GetStoriesJson(_storiesFeed);
            _ids = await _api.DeserializeAsync<List<int>>(json);
            if (_ids is null)
            {
                return;
            }

            var index = 1;

            foreach (var id in _ids)
            {
                var itemViewModel = new ItemViewModel(_api, _navigation, id, index++);
                Items.Add(itemViewModel);
            }
        }
    }

    public async Task UpdateAsync()
    {
        // TODO:
        await Task.Yield();
    }

    public async Task<bool> BackAsync()
    {
        // TODO:
        return await Task.FromResult(false);
    }
}
