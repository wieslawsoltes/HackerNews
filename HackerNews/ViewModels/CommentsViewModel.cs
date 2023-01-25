using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using HackerNews.Model;
using HackerNews.Services;

namespace HackerNews.ViewModels;

public partial class CommentsViewModel : ViewModelBase, ILazyLoadable
{
    private readonly HackerNewsApiV0? _api;
    private readonly INavigation? _navigation;
    
    [ObservableProperty] private ItemViewModel? _itemViewModel;

    public CommentsViewModel()
    {
    }

    public CommentsViewModel(HackerNewsApiV0 api, INavigation navigation, ItemViewModel itemViewModel)
    {
        _api = api;
        _navigation = navigation;
        _itemViewModel = itemViewModel;
    }

    public bool IsLoaded()
    {
        return _itemViewModel?.Kids is { };
    }

    public async Task LoadAsync()
    {
        if (_itemViewModel is { })
        {
            await _itemViewModel.LoadKidsAsync();
        }
    }

    public async Task UpdateAsync()
    {
        if (_itemViewModel?.Kids is { })
        {
            foreach (var kid in _itemViewModel.Kids)
            {
                await kid.UpdateAsync();
            }
        }
    }

    public async Task<bool> BackAsync()
    {
        // TODO:
        return await Task.FromResult(false);
    }
}
