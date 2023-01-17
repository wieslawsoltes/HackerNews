using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HackerNews.ViewModels;

public partial class MainViewViewModel : ViewModelBase
{
    private List<int>? _topStoriesIds;

    [ObservableProperty] private ObservableCollection<ItemViewModel> _items;

    public MainViewViewModel()
    {
        _items = new ObservableCollection<ItemViewModel>();
        LoadItemsCommand = new AsyncRelayCommand(LoadItems);
    }

    public IAsyncRelayCommand LoadItemsCommand { get; }

    private async Task LoadItems()
    {
        _items.Clear();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var client = new HttpClient();

        var topStoriesJson = await GetTopStoriesJson(client);
        _topStoriesIds = JsonSerializer.Deserialize<List<int>>(topStoriesJson, options);
        if (_topStoriesIds is null)
        {
            return;
        }

        var storiesIds = _topStoriesIds.Take(20);
        var index = 1;

        foreach (var id in storiesIds) 
        {
            var itemJson = await GetItemJson(id, client);
            var item = default(ItemViewModel);

            await Task.Run(() =>
            {
                item = JsonSerializer.Deserialize<ItemViewModel>(itemJson, options);
                if (item is { })
                {
                    item.Index = index++;
                    _items.Add(item);
                }
            });

#if false
            if (item?.By is { })
            {
                var userJson = await GetUserJson(item.By, client);
                var user = JsonSerializer.Deserialize<UserViewModel>(userJson, options);
                if (user is { })
                {
                    Debug.WriteLine(userJson);
                }
            }
#endif
        }
    }

    private const string UriPrefix = "https://hacker-news.firebaseio.com/v0/";

    private static async Task<string> GetTopStoriesJson(HttpClient client)
    {
        var requestUri = $"{UriPrefix}/topstories.json?print=pretty";
        var response = await client.GetAsync(requestUri);
        var json = await response.Content.ReadAsStringAsync();
        return json;
    }

    private static async Task<string> GetItemJson(int id, HttpClient client)
    {
        var requestUri = $"{UriPrefix}/item/{id}.json?print=pretty";
        var response = await client.GetAsync(requestUri);
        var json = await response.Content.ReadAsStringAsync();
        return json;
    }  

    private static async Task<string> GetUserJson(string username, HttpClient client)
    {
        var requestUri = $"{UriPrefix}/user/{username}.json?print=pretty";
        var response = await client.GetAsync(requestUri);
        var json = await response.Content.ReadAsStringAsync();
        return json;
    }
}
