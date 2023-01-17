using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using HackerNews.Model;
using HackerNews.Services;

namespace HackerNews.ViewModels;

public partial class ItemViewModel : ViewModelBase
{
    private readonly HackerNewsApiV0 _api;
    private Item? _item;
    [ObservableProperty] private int _index;
    [ObservableProperty] private int _id;
    [ObservableProperty] private bool _deleted;
    [ObservableProperty] private string _type;
    [ObservableProperty] private UserViewModel? _by;
    [ObservableProperty] private int _time;
    [ObservableProperty] private string _text;
    [ObservableProperty] private bool _dead;
    [ObservableProperty] private int? _parent;
    [ObservableProperty] private int? _poll;
    [ObservableProperty] private List<int> _kids;
    [ObservableProperty] private string _url;
    [ObservableProperty] private int _score;
    [ObservableProperty] private string _title;
    [ObservableProperty] private List<int> _parts;
    [ObservableProperty] private int? _descendants;

    public ItemViewModel(HackerNewsApiV0 api, int index, int id)
    {
        _api = api;
        _index = index;
        _id = id;
    }

    public bool IsLoaded()
    {
        return _item is { };
    }

    public async Task Load()
    {
        if (_item is null)
        {
            var json = await _api.GetItemJson(_id);
            _item = await _api.DeserializeAsync<Item>(json);
        }
    }

    public async Task LoadUser()
    {
        if (_item?.By is { })
        {
            var json = await _api.GetUserJson(_item.By);
            var user = await _api.DeserializeAsync<User>(json);
            if (user is { })
            {
                By = new UserViewModel(_api, user);
            }
        }
    }

    public void Update()
    {
        if (_item is { })
        {
            Score = _item.Score;
            Title = _item.Title;
            // TODO:
        }
    }
}
