using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using HackerNews.Converters;
using HackerNews.Model;
using HackerNews.Model.Html;

namespace HackerNews.ViewModels;

public partial class ItemViewModel : ViewModelBase, ILazyLoadable
{
    private Item? _item;

    [ObservableProperty] private bool _isVisible;
    [ObservableProperty] private int _index;
    [ObservableProperty] private bool _isViewed;
    [ObservableProperty] private bool _isExpanded;
    [ObservableProperty] private int _level;

    [ObservableProperty] private int _id;
    [ObservableProperty] private bool _deleted;
    [ObservableProperty] private ItemType _type;
    [ObservableProperty] private UserViewModel? _by;
    [ObservableProperty] private string? _byId;
    [ObservableProperty] private DateTimeOffset _time;
    [ObservableProperty] private string? _timeAgo;
    [ObservableProperty] private string? _text;
    [ObservableProperty] private Node? _textNode;
    [ObservableProperty] private bool _dead;
    [ObservableProperty] private int? _parent;
    [ObservableProperty] private int? _poll;
    [ObservableProperty] private ItemListViewModel? _kids;
    [ObservableProperty] private Uri? _url;
    [ObservableProperty] private int _score;
    [ObservableProperty] private string? _title;
    [ObservableProperty] private ItemListViewModel? _parts;
    [ObservableProperty] private int? _descendants;

    public ItemViewModel() : this(-1)
    {
    }

    public ItemViewModel(int id, int index = -1, int level = 0)
    {
        _id = id;
        _index = index;
        _isViewed = false;
        _isExpanded = false;
        _level = level;
 
        LoadUserCommand = new AsyncRelayCommand(async () => await LoadUser());

        LoadKidsCommand = new AsyncRelayCommand(async () => await LoadKids());

        LoadPartsCommand = new AsyncRelayCommand(async () => await LoadParts());

        RefreshCommand = new AsyncRelayCommand(async () => await Refresh());

        CommentCommand = new AsyncRelayCommand(async () => await Comment());

        ShareCommand = new AsyncRelayCommand(async () => await Share());

        VoteCommand = new AsyncRelayCommand(async () => await Vote());

        SaveCommand = new AsyncRelayCommand(async () => await Save());

        OpenUrlCommand = new AsyncRelayCommand(async () => await OpenUrl());

        ToggleIsExpandedCommand = new AsyncRelayCommand(async () => await ToggleIsExpanded());

        LoadIsViewed();
    }

    public IAsyncRelayCommand? ToggleIsExpandedCommand { get; }

    public IAsyncRelayCommand? LoadUserCommand { get; }

    public IAsyncRelayCommand? LoadKidsCommand { get; }

    public IAsyncRelayCommand? LoadPartsCommand { get; }

    public IAsyncRelayCommand? RefreshCommand { get; }
    
    public IAsyncRelayCommand? CommentCommand { get; }
    
    public IAsyncRelayCommand? ShareCommand { get; }

    public IAsyncRelayCommand? VoteCommand { get; }
    
    public IAsyncRelayCommand? SaveCommand { get; }

    public IAsyncRelayCommand? OpenUrlCommand { get; }

    public bool IsLoaded()
    {
        return _item is { };
    }

    public async Task LoadAsync()
    {
        var api = Ioc.Default.GetService<IHackerNewsService>();

        if (api is { } && _item is null && Id >= 0)
        {
            try
            {
                var json = await api.GetItemJson(Id);
                _item = await api.DeserializeAsync<Item>(json);
            }
            catch (Exception e)
            {
                Ioc.Default.GetService<ILog>()?.Log(e);
            }
        }
    }

    public async Task UpdateAsync()
    {
        if (_item is { })
        {
            Deleted = _item.Deleted;

            if (_item.Type is { })
            {
                Type = StringConverter.ItemTypeFromString(_item.Type);
            }

            ById = _item.By;

            Time = DateTimeOffset.FromUnixTimeSeconds(_item.Time);
            TimeAgo = StringConverter.ToTimeAgoString(Time);

            var node = HtmlConverter.ParseHtml(_item.Text);
            Text = HtmlConverter.ToString(node);
            TextNode = node;

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
        var navigation = Ioc.Default.GetService<INavigationService>();
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
            Kids ??= new ItemListViewModel();
            Kids.Clear();

            await LoadItemsAsync(Kids, _item.Kids, Level + 1);
        }
    }

    public async Task LoadKPartsAsync()
    {
        if (_item is { } && _item.Parts is { })
        {
            Parts ??= new ItemListViewModel();
            Parts.Clear();

            await LoadItemsAsync(Parts, _item.Parts, Level + 1);
        }
    }

    private async Task LoadUser()
    {
        if (By is null)
        {
            if (_item?.By is { })
            {
                By = new UserViewModel(_item.By);
            }
        }

        if (By is { })
        {
            var navigation = Ioc.Default.GetService<INavigationService>();
            if (navigation is { })
            {
                await navigation.NavigateAsync(By);
            }
        }
    }

    private async Task LoadKids()
    {
        var navigation = Ioc.Default.GetService<INavigationService>();
        if (navigation is { })
        {
            IsViewed = true;

            var stateManager = Ioc.Default.GetService<IStateStorageService>();
            if (stateManager is { })
            {
                stateManager.SetIsViewed(Id);
            }

            IsExpanded = true;

            var commentsViewModel = new CommentsViewModel(this);

            await navigation.NavigateAsync(commentsViewModel);
        }
    }

    private async Task LoadParts()
    {
        var navigation = Ioc.Default.GetService<INavigationService>();
        if (navigation is { })
        {
            var pollViewModel = new PollViewModel(this);

            await navigation.NavigateAsync(pollViewModel);
        }
    }

    private async Task Refresh()
    {
        _item = null;
        await LoadAsync();
        await UpdateAsync();
    }

    private async Task Comment()
    {
        // TODO: Comment
        await Task.Yield();
    }

    private async Task Share()
    {
        if (Url is { })
        {
            var share = Ioc.Default.GetService<IShareService>();
            if (share is { })
            {
                await share.ShareTextAsync(
                    "Share Url", 
                    Title ?? "", 
                    Url.ToString());
            }
        }
    }

    private async Task Vote()
    {
        // TODO: Vote
        await Task.Yield();
    }

    private async Task Save()
    {
        // TODO: Save
        await Task.Yield();
    }

    private async Task OpenUrl()
    {
        if (Url is { })
        {
            var browser = Ioc.Default.GetService<IBrowserService>();
            if (browser is { })
            {
                await browser.OpenBrowserAsync(Url);
            }
        }
    }

    private void LoadIsViewed()
    {
        var stateManager = Ioc.Default.GetService<IStateStorageService>();
        if (stateManager is { })
        {
            IsViewed = stateManager.GetIsViewed(Id);
        }
    }

    private async Task ToggleIsExpanded()
    {
        IsExpanded = !IsExpanded;
        await Task.Yield();
    }

    public static async Task LoadItemsAsync(ItemListViewModel items, List<int> ids, int level)
    {
        var index = 1;

        foreach (var id in ids)
        {
            var itemViewModel = new ItemViewModel(id, index++, level);

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
    
    public override string ToString()
    {
        return $"{Index}";
    }
}
