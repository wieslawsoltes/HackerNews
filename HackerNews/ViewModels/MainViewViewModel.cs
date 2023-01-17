using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        var client = new HttpClient();
        var urlTopStories = "https://hacker-news.firebaseio.com/v0/topstories.json?print=pretty";
        var response = await client.GetAsync(urlTopStories);
        var json = await response.Content.ReadAsStringAsync();
        _topStoriesIds = JsonSerializer.Deserialize<List<int>>(json);

        var storiesIds = _topStoriesIds.Take(20);
        var index = 1;

        foreach (var id in storiesIds) 
        {
            var urlItemId = $"https://hacker-news.firebaseio.com/v0/item/{id}.json?print=pretty";
            var itemResponse = await client.GetAsync(urlItemId);
            var itemJson = await itemResponse.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            await Task.Run(() =>
            {
                var item = JsonSerializer.Deserialize<ItemViewModel>(itemJson, options);
                item.Index = index++;
                _items.Add(item);
            });
        }
    }
}
