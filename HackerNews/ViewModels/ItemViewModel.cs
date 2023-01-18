using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using HackerNews.Model;
using HackerNews.Services;

namespace HackerNews.ViewModels;

public partial class ItemViewModel : ViewModelBase, ILazyLoadable
{
    private readonly HackerNewsApiV0? _api;
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

    public ItemViewModel()
    {
        _id = -1;
    }

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

    public async Task LoadAsync()
    {
        if (_api is { } && _item is null && _id >= 0)
        {
            var json = await _api.GetItemJson(_id);
            _item = await _api.DeserializeAsync<Item>(json);
        }
    }

    public async Task LoadUser()
    {
        if (_api is { } && _item?.By is { })
        {
            _by = new UserViewModel(_api, _item.By);
            await _by.LoadAsync();
        }
    }

    public async Task UpdateAsync()
    {
        if (_item is { })
        {
            Score = _item.Score;
            Title = _item.Title;
            // TODO:
        }

        // TODO:
        await Task.Yield();
    }

    public async Task BackAsync()
    {
        // TODO:
        await Task.Yield();
    }

    public override string ToString()
    {
        //return base.ToString();
        return $"{Index}";
    }
}
