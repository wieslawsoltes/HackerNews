using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using HackerNews.Model;

namespace HackerNews.ViewModels;

public partial class ItemViewModel : ViewModelBase, ILazyLoadable
{
    private Item? _item;

    [ObservableProperty] private bool _isVisible;
    [ObservableProperty] private int _index;
    [ObservableProperty] private bool _isViewed;

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

    public ItemViewModel(int id, int index = -1)
    {
        var stateManager = Ioc.Default.GetService<IStateManager>();

        _id = id;
        _index = index;
  
        if (stateManager is { })
        {
            _isViewed = stateManager.GetIsViewed(id);
        }

        LoadUserCommand = new AsyncRelayCommand(async () =>
        {
            var api = Ioc.Default.GetService<IHackerNewsApi>();
            var navigation = Ioc.Default.GetService<INavigation>();

            if (navigation is { })
            {
                if (_by is null)
                {
                    if (api is { } && _item?.By is { })
                    {
                        By = new UserViewModel(_item.By);
                    }
                }

                if (_by is { })
                {
                    await navigation.NavigateAsync(_by);
                }
            }
        });

        LoadKidsCommand = new AsyncRelayCommand(async () =>
        {
            var navigation = Ioc.Default.GetService<INavigation>();
            if (navigation is { })
            {
                IsViewed = true;

                if (stateManager is { })
                {
                    stateManager.SetIsViewed(Id);
                }
                
                var commentsViewModel = new CommentsViewModel(this);

                await navigation.NavigateAsync(commentsViewModel);
            }
        });

        LoadPartsCommand = new AsyncRelayCommand(async () =>
        {
            var navigation = Ioc.Default.GetService<INavigation>();
            if (navigation is { })
            {
                var pollViewModel = new PollViewModel(this);

                await navigation.NavigateAsync(pollViewModel);
            }
        });

        VoteCommand = new AsyncRelayCommand(async () =>
        {
            // TODO:
            await Task.Yield();
        });

        SaveCommand = new AsyncRelayCommand(async () =>
        {
            // TODO:
            await Task.Yield();
        });

        OpenUrlCommand = new AsyncRelayCommand(async () =>
        {
            if (Url is { })
            {
                await Task.Run(() =>
                {
                    // TODO: Add support for Android, iOS etc.
                    OpenUrl(Url.ToString());
                });
            }
        });
    }

    public IAsyncRelayCommand? LoadUserCommand { get; }

    public IAsyncRelayCommand? LoadKidsCommand { get; }

    public IAsyncRelayCommand? LoadPartsCommand { get; }

    public IAsyncRelayCommand? VoteCommand { get; }
    
    public IAsyncRelayCommand? SaveCommand { get; }

    public IAsyncRelayCommand? OpenUrlCommand { get; }

    public bool IsLoaded()
    {
        return _item is { };
    }

    public async Task LoadAsync()
    {
        var api = Ioc.Default.GetService<IHackerNewsApi>();

        if (api is { } && _item is null && Id >= 0)
        {
            var json = await api.GetItemJson(Id);
            _item = await api.DeserializeAsync<Item>(json);
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

    public async Task<bool> BackAsync()
    {
        var navigation = Ioc.Default.GetService<INavigation>();
        if (navigation is { })
        {
            return await navigation.BackAsync();
        }

        return await Task.FromResult(false);
    }
    
    public async Task LoadKidsAsync()
    {
        if (_item is { } && _item.Kids is { })
        {
            Kids ??= new ObservableCollection<ItemViewModel>();
            Kids.Clear();

            await LoadItemsAsync(Kids, _item.Kids);
        }
    }

    public async Task LoadKPartsAsync()
    {
        if (_item is { } && _item.Parts is { })
        {
            Parts ??= new ObservableCollection<ItemViewModel>();
            Parts.Clear();

            await LoadItemsAsync(Parts, _item.Parts);
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

    public static async Task LoadItemsAsync(ObservableCollection<ItemViewModel> items, List<int> ids)
    {
        var index = 1;

        foreach (var id in ids)
        {
            var itemViewModel = new ItemViewModel(id, index++);

            items.Add(itemViewModel);
        }

        // TODO: Await the Task
        var _ = Task.Run(async () =>
        {
            foreach (var kid in items)
            {
                await kid.LoadAsync();
                await kid.LoadKidsAsync();
            }
        });
        await Task.Yield();
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

    public static void OpenUrl(string url)
    {
        try
        {
            Process.Start(url);
        }
        catch
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                url = url.Replace("&", "^&");
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
            else
            {
                throw;
            }
        }
    }
}
