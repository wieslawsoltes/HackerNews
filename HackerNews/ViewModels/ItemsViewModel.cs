using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using HackerNews.Model;
using HackerNews.Services;

namespace HackerNews.ViewModels;

public partial class ItemsViewModel : ViewModelBase, ILazyLoadable
{
    private readonly HackerNewsApiV0? _api;
    private readonly string? _storiesFeed;
    private List<int>? _ids;
    [ObservableProperty] private string? _title;
    [ObservableProperty] private ObservableCollection<ItemViewModel> _items;

    public ItemsViewModel()
    {
    }

    public ItemsViewModel(HackerNewsApiV0 api, string storiesFeed, string title)
    {
        _api = api;
        _storiesFeed = storiesFeed;
        _title = title;
        _ids = new List<int>();
        _items = new ObservableCollection<ItemViewModel>();
    }

    public bool IsLoaded()
    {
        // TODO:
        return true;
    }

    public async Task LoadAsync()
    {
        if (_api is { } && _storiesFeed is { })
        {
            _ids ??= new List<int>();
            _ids.Clear();
            _items.Clear();

            var json = await _api.GetStoriesJson(_storiesFeed);
            _ids = await _api.DeserializeAsync<List<int>>(json);
            if (_ids is null)
            {
                return;
            }

            var index = 1;

            foreach (var id in _ids)
            {
                var itemViewModel = new ItemViewModel(_api, index++, id);
                _items.Add(itemViewModel);
            }
        }
    }

    public async Task UpdateAsync()
    {
        // TODO:
        await Task.Yield();
    }

    public async Task BackAsync()
    {
        // TODO:
        await Task.Yield();
    }
}
