using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Web;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HackerNews.Model;
using HackerNews.Services;

namespace HackerNews.ViewModels;

public partial class ItemViewModel : ViewModelBase, ILazyLoadable
{
    private readonly HackerNewsApiV0? _api;
    private readonly INavigation? _navigation;
    private Item? _item;

    [ObservableProperty] private int _index;
    [ObservableProperty] private int _id;
    [ObservableProperty] private bool _deleted;
    [ObservableProperty] private ItemType _type;
    [ObservableProperty] private UserViewModel? _by;
    [ObservableProperty] private string? _byId;
    [ObservableProperty] private DateTimeOffset _time;
    [ObservableProperty] private string? _timeAgo;
    [ObservableProperty] private string? _text;
    [ObservableProperty] private bool _dead;
    [ObservableProperty] private int? _parent;
    [ObservableProperty] private int? _poll;
    [ObservableProperty] private ObservableCollection<ItemViewModel>? _kids;
    [ObservableProperty] private Uri? _url;
    [ObservableProperty] private int _score;
    [ObservableProperty] private string? _title;
    [ObservableProperty] private ObservableCollection<ItemViewModel>? _parts;
    [ObservableProperty] private int? _descendants;

    public ItemViewModel()
    {
        _id = -1;
        _index = -1;
    }

    public ItemViewModel(HackerNewsApiV0 api, INavigation navigation, int id, int index = -1)
    {
        _api = api;
        _navigation = navigation;
        _id = id;
        _index = index;

        LoadUserCommand = new AsyncRelayCommand(async () =>
        {
            if (_navigation is { })
            {
                if (_by is null)
                {
                    if (_api is { } && _item?.By is { })
                    {
                        By = new UserViewModel(_api, _navigation, _item.By);
                    }
                }

                if (_by is { })
                {
                    await _navigation.NavigateAsync(_by);
                }
            }
        });

        LoadKidsCommand = new AsyncRelayCommand(async () =>
        {
            if (_navigation is { })
            {
                var commentsViewModel = new CommentsViewModel(_api, _navigation, this);

                await _navigation.NavigateAsync(commentsViewModel);
            }
        });

        LoadPartsCommand = new AsyncRelayCommand(async () =>
        {
            if (_navigation is { })
            {
                var pollViewModel = new PollViewModel(_api, _navigation, this);

                await _navigation.NavigateAsync(pollViewModel);
            }
        });
    }

    public IAsyncRelayCommand LoadUserCommand { get; }

    public IAsyncRelayCommand LoadKidsCommand { get; }

    public IAsyncRelayCommand LoadPartsCommand { get; }

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

    public async Task UpdateAsync()
    {
        if (_item is { })
        {
            Deleted = _item.Deleted;

            if (_item.Type is { })
            {
                Type = ItemTypeFromString(_item.Type);
            }

            ById = _item.By;

            Time = DateTimeOffset.FromUnixTimeSeconds(_item.Time);
            TimeAgo = ToTimeAgoString(Time);
            Text = ParseHtmlString(_item.Text);
            Dead = _item.Dead;
            Parent = _item.Parent;
            Poll = _item.Poll;

            if (_item.Url is { })
            {
                Url = new Uri(_item.Url);
            }

            Score = _item.Score;
            Title = _item.Title;

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
    
    public async Task LoadKidsAsync()
    {
        if (_api is { } && _navigation is { } && _item is { } && _item.Kids is { })
        {
            Kids ??= new ObservableCollection<ItemViewModel>();
            Kids.Clear();

            await LoadItemsAsync(Kids, _item.Kids, _api, _navigation);
        }
    }

    public async Task LoadKPartsAsync()
    {
        if (_api is { } && _navigation is { } && _item is { } && _item.Parts is { })
        {
            Parts ??= new ObservableCollection<ItemViewModel>();
            Parts.Clear();

            await LoadItemsAsync(Parts, _item.Parts, _api, _navigation);
        }
    }

    public override string ToString()
    {
        return $"{Index}";
    }

    public static string? ParseHtmlString(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return text;
        }

        var decoded = HttpUtility.HtmlDecode(text);

        var breaks = decoded.Replace("<p>", $"{Environment.NewLine}{Environment.NewLine}");

        return breaks;
    }

    public static async Task LoadItemsAsync(ObservableCollection<ItemViewModel> items, List<int> ids, HackerNewsApiV0 api, INavigation navigation)
    {
        var index = 1;

        foreach (var id in ids)
        {
            var itemViewModel = new ItemViewModel(api, navigation, id, index++);

            items.Add(itemViewModel);
        }

        foreach (var kid in items)
        {
            await kid.LoadAsync();
            await kid.LoadKidsAsync();
        }
    }

    public static string ToTimeAgoString(DateTimeOffset dto)
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

    public static ItemType ItemTypeFromString(string type)
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
}
