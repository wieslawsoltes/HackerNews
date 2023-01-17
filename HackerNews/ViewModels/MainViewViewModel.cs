using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HackerNews.Model;

namespace HackerNews.ViewModels;

public partial class MainViewViewModel : ViewModelBase
{
    private List<int> _topStoriesIds;
    private List<Item> _topStoriesItems;
    [ObservableProperty] private ObservableCollection<ItemViewModel> _items;

    public MainViewViewModel()
    {
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

        var client = new HttpClient();

        var topStoriesJson = await GetTopStoriesJson(client);
        _topStoriesIds = await JsonSerializer.DeserializeAsync<List<int>>(topStoriesJson, options);
        if (_topStoriesIds is null)
        {
            return;
        }

        var storiesIds = _topStoriesIds.Take(20);
        var index = 1;

        foreach (var id in storiesIds) 
        {
            var itemJson = await GetItemJson(id, client);
            var item = default(Item);

            item = await JsonSerializer.DeserializeAsync<Item>(itemJson, options);
            if (item is { })
            {
                item.Index = index++;
                _topStoriesItems.Add(item);

                var itemViewModel = new ItemViewModel(item);
                itemViewModel.Update();
                _items.Add(itemViewModel);
            }

#if false
            if (item?.By is { })
            {
                var userJson = await GetUserJson(item.By, client);
                var user = JsonSerializer.Deserialize<User>(userJson, options);
                if (user is { })
                {
                    // var userViewModel = new UserViewModel(user);
                    Debug.WriteLine(userJson);
                }
            }
#endif
        }
    }

    private const string UriPrefix = "https://hacker-news.firebaseio.com/v0/";

    private static async Task<Stream> GetTopStoriesJson(HttpClient client)
    {
        var requestUri = $"{UriPrefix}/topstories.json?print=pretty";
        var response = await client.GetAsync(requestUri);
        var json = await response.Content.ReadAsStreamAsync();
        return json;
    }

    private static async Task<Stream> GetItemJson(int id, HttpClient client)
    {
        var requestUri = $"{UriPrefix}/item/{id}.json?print=pretty";
        var response = await client.GetAsync(requestUri);
        var json = await response.Content.ReadAsStreamAsync();
        return json;
    }  

    private static async Task<Stream> GetUserJson(string username, HttpClient client)
    {
        var requestUri = $"{UriPrefix}/user/{username}.json?print=pretty";
        var response = await client.GetAsync(requestUri);
        var json = await response.Content.ReadAsStreamAsync();
        return json;
    }
}
