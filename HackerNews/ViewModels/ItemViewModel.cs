using System;
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
    [ObservableProperty] private ItemType _type;
    [ObservableProperty] private UserViewModel? _by;
    [ObservableProperty] private DateTimeOffset _time;
    [ObservableProperty] private string _timeAgo;
    [ObservableProperty] private string _text;
    [ObservableProperty] private bool _dead;
    [ObservableProperty] private int? _parent;
    [ObservableProperty] private int? _poll;
    [ObservableProperty] private List<int> _kids;
    [ObservableProperty] private Uri _url;
    [ObservableProperty] private int _score;
    [ObservableProperty] private string _title;
    [ObservableProperty] private List<int> _parts;
    [ObservableProperty] private int? _descendants;

    public ItemViewModel()
    {
        _id = -1;
        _index = -1;
    }

    public ItemViewModel(HackerNewsApiV0 api, int id, int index = -1)
    {
        _api = api;
        _id = id;
        _index = index;
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

            await LoadUser();

            // TODO: Load Kids as comment view models (Item based).
            // Kids = new List<int>(_item.Kids);

            // TODO: Load Parts as poll options view models (Item based).
            // Kids = new List<int>(_item.Kids);
        }
    }

    public async Task LoadUser()
    {
        if (_api is { } && _item?.By is { })
        {
            By = new UserViewModel(_api, _item.By);
            await By.LoadAsync();
        }
    }

    public async Task UpdateAsync()
    {
        if (_item is { })
        {
            Deleted = _item.Deleted;
            Type = ItemTypeFromString(_item.Type);
            // TODO: By
            Time = DateTimeOffset.FromUnixTimeSeconds(_item.Time);
            TimeAgo = ToTimeAgoString(Time);
            Text = _item.Text;
            Dead = _item.Dead;
            Parent = _item.Parent;
            Poll = _item.Poll;
            // TODO: Kids
            Url = new Uri(_item.Url);
            Score = _item.Score;
            Title = _item.Title;
            // TODO: Parts
            Descendants = _item.Descendants;
        }

        // TODO:
        await Task.Yield();
    }

    public async Task BackAsync()
    {
        // TODO:
        await Task.Yield();
    }

    private string ToTimeAgoString(DateTimeOffset dto)
    {
        var ts = DateTimeOffset.Now - dto;
        if (ts.Days > 0)
        {
            return $"{ts.Days}d";
        }
        else if (ts.Hours > 0)
        {
            return $"{ts.Hours}h";
        }
        else if (ts.Minutes > 0)
        {
            return $"{ts.Minutes}m";
        }
        else if (ts.Seconds > 0)
        {
            return $"{ts.Seconds}s";
        }
        else
        {
            return $"{ts.Milliseconds}ms";
        }
    }

    private ItemType ItemTypeFromString(string type)
    {
        switch (type)
        {
            case "job": return ItemType.Job;
            case "story": return ItemType.Story;
            case "comment": return ItemType.Comment;
            case "poll": return ItemType.Poll;
            case "pollopt": return ItemType.PollOpt;
        }

        throw new Exception("Invalid item type.");
    }

    public override string ToString()
    {
        //return base.ToString();
        return $"{Index}";
    }
}
