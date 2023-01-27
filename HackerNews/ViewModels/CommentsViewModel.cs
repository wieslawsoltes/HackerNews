using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using HackerNews.Model;

namespace HackerNews.ViewModels;

public partial class CommentsViewModel : ViewModelBase, ILazyLoadable
{
    [ObservableProperty] private ItemViewModel? _itemViewModel;

    public CommentsViewModel()
    {
    }

    public CommentsViewModel(ItemViewModel itemViewModel)
    {
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
        var navigation = Ioc.Default.GetService<INavigation>();
        if (navigation is { })
        {
            return await navigation.BackAsync();
        }

        return await Task.FromResult(false);
    }
}
