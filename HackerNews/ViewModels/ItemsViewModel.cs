using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using HackerNews.Model;

namespace HackerNews.ViewModels;

public partial class ItemsViewModel : ViewModelBase, ILazyLoadable
{
    private readonly string? _storiesFeed;
    private List<int>? _ids;

    [ObservableProperty] private bool _isVisible;
    [ObservableProperty] private string? _title;
    [ObservableProperty] private string? _icon;
    [ObservableProperty] private ItemListViewModel? _items;

    public ItemsViewModel() : this(null, null, null, null)
    {
    }

    public ItemsViewModel(string? storiesFeed, string? title, string? icon, Func<ItemsViewModel, Task>? openFeed)
    {
        _storiesFeed = storiesFeed;
        _title = title;
        _icon = icon;

        if (openFeed is { })
        {
            OpenFeedCommand = new AsyncRelayCommand(async () => await openFeed(this));
        }
    }

    public IAsyncRelayCommand? OpenFeedCommand { get; }

    public bool IsLoaded()
    {
        // TODO:
        return false;
    }

    public async Task LoadAsync()
    {
        var serializer = Ioc.Default.GetService<ISerializerService>();
        var api = Ioc.Default.GetService<IHackerNewsService>();

        if (api is { } && serializer is { } && _storiesFeed is { })
        {
            _ids ??= new List<int>();
            _ids.Clear();

            Items ??= new ItemListViewModel();
            Items.Clear();

            try
            {
                var json = await api.GetStoriesJson(_storiesFeed);
                _ids = await serializer.DeserializeAsync<List<int>>(json);
                if (_ids is null)
                {
                    return;
                }
            }
            catch (Exception e)
            {
                Ioc.Default.GetService<ILog>()?.Log(e);
                return;
            }

            var index = 1;

            foreach (var id in _ids)
            {
                var itemViewModel = new ItemViewModel(id, index++);
                Items.Add(itemViewModel);
            }
        }
    }

    public async Task UpdateAsync()
    {
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
}
