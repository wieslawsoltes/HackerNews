using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using HackerNews.Model;

namespace HackerNews.ViewModels;

public partial class ItemsViewModel : ViewModelBase, ILazyLoadable
{
    private readonly string? _storiesFeed;
    private List<int>? _ids;

    [ObservableProperty] private bool _isVisible;
    [ObservableProperty] private string? _title;
    [ObservableProperty] private ObservableCollection<ItemViewModel>? _items;

    public ItemsViewModel()
    {
    }

    public ItemsViewModel(string storiesFeed, string title)
    {
        _storiesFeed = storiesFeed;
        _title = title;
    }

    public bool IsLoaded()
    {
        // TODO:
        return false;
    }

    public async Task LoadAsync()
    {
        var api = Ioc.Default.GetService<IHackerNewsService>();
        var navigation = Ioc.Default.GetService<INavigationService>();

        if (api is { } && navigation is { } && _storiesFeed is { })
        {
            _ids ??= new List<int>();
            _ids.Clear();

            Items ??= new ObservableCollection<ItemViewModel>();
            Items.Clear();

            var json = await api.GetStoriesJson(_storiesFeed);
            _ids = await api.DeserializeAsync<List<int>>(json);
            if (_ids is null)
            {
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
