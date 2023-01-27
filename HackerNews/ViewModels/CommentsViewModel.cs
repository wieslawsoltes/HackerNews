using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using HackerNews.Model;

namespace HackerNews.ViewModels;

public partial class CommentsViewModel : ViewModelBase, ILazyLoadable
{
    private readonly IHackerNewsApi? _api;
    private readonly INavigation? _navigation;
    
    [ObservableProperty] private ItemViewModel? _itemViewModel;

    public CommentsViewModel()
    {
    }

    public CommentsViewModel(ItemViewModel itemViewModel)
    {
        _api = Ioc.Default.GetService<IHackerNewsApi>();
        _navigation = Ioc.Default.GetService<INavigation>();
        _itemViewModel = itemViewModel;
    }

    public bool IsLoaded()
    {
        return ItemViewModel?.Kids is { };
    }

    public async Task LoadAsync()
    {
        if (ItemViewModel is { })
        {
            await ItemViewModel.LoadKidsAsync();
        }
    }

    public async Task UpdateAsync()
    {
        if (ItemViewModel?.Kids is { })
        {
            foreach (var kid in ItemViewModel.Kids)
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
