using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HackerNews.Model;
using HackerNews.Services;

namespace HackerNews.ViewModels;

public partial class MainViewViewModel : ViewModelBase
{
    private HackerNewsApiV0 _api;
    private HttpClient _client;
    private List<int> _topStoriesIds;
    private List<Item> _topStoriesItems;
    [ObservableProperty] private ObservableCollection<ItemViewModel> _items;

    public MainViewViewModel()
    {
        _api = new HackerNewsApiV0();
        _client = new HttpClient();
        _topStoriesIds = new List<int>();
        _topStoriesItems = new List<Item>();
        _items = new ObservableCollection<ItemViewModel>();

        LoadItemsCommand = new AsyncRelayCommand(LoadItems);
    }

    public IAsyncRelayCommand LoadItemsCommand { get; }

    private async Task LoadItems()
    {
        _topStoriesItems.Clear();
        _items.Clear();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        
        var topStoriesJson = await _api.GetTopStoriesJson(_client);
        _topStoriesIds = await JsonSerializer.DeserializeAsync<List<int>>(topStoriesJson, options);
        if (_topStoriesIds is null)
        {
            return;
        }

        var storiesIds = _topStoriesIds.Take(20);
        var index = 1;

        foreach (var id in storiesIds) 
        {
            var itemJson = await _api.GetItemJson(id, _client);
            var item = await JsonSerializer.DeserializeAsync<Item>(itemJson, options);
            var itemViewModel = default(ItemViewModel);
            if (item is { })
            {
                item.Index = index++;
                _topStoriesItems.Add(item);
                
                itemViewModel = new ItemViewModel(item);
                itemViewModel.Update();
                _items.Add(itemViewModel);
            }

#if false
            if (itemViewModel is { } && item?.By is { })
            {
                var userJson = await _api.GetUserJson(item.By, _client);
                var user = await JsonSerializer.DeserializeAsync<User>(userJson, options);
                if (user is { })
                {
                    itemViewModel.By = new UserViewModel(user);
                    //Debug.WriteLine(userJson);
                }
            }
#endif
        }
    }
}
