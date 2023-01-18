using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HackerNews.Model;
using HackerNews.Services;

namespace HackerNews.ViewModels;

public partial class MainViewViewModel : ViewModelBase, ILazyLoadable
{
    private readonly HackerNewsApiV0 _api;
    private List<int> _topStoriesIds;
    [ObservableProperty] private ObservableCollection<ItemViewModel> _items;

    public MainViewViewModel()
    {
        _api = new HackerNewsApiV0();
        _topStoriesIds = new List<int>();
        _items = new ObservableCollection<ItemViewModel>();

        LoadItemsCommand = new AsyncRelayCommand(Load);
    }

    public IAsyncRelayCommand LoadItemsCommand { get; }

    public bool IsLoaded()
    {
        // TODO:
        return true;
    }

    public async Task Load()
    {
        _topStoriesIds.Clear();
        _items.Clear();

        var json = await _api.GetTopStoriesJson();
        _topStoriesIds = await _api.DeserializeAsync<List<int>>(json);
        if (_topStoriesIds is null)
        {
            return;
        }

        var index = 1;

        foreach (var id in _topStoriesIds)
        {
            var itemViewModel = new ItemViewModel(_api, index++, id);
            _items.Add(itemViewModel);
        }
    }

    public void Update()
    {
        // TODO:
    }

    public async Task Back()
    {
        // TODO:
        await Task.Yield();
    }
}
