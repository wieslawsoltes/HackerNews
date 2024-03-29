using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using HackerNews.Model;

namespace HackerNews.ViewModels;

public partial class CommentsViewModel : ViewModelBase, ILazyLoadable
{
    [ObservableProperty] private bool _isVisible;
    [ObservableProperty] private ItemViewModel? _item;

    public CommentsViewModel() : this(null)
    {
    }

    public CommentsViewModel(ItemViewModel? item = null)
    {
        _item = item;
    }

    public bool IsLoaded()
    {
        return Item?.Kids is { };
    }

    public async Task LoadAsync()
    {
        if (Item is { })
        {
            await Item.LoadKidsAsync();
        }
    }

    public async Task UpdateAsync()
    {
        if (Item?.Kids is { })
        {
            foreach (var kid in Item.Kids)
            {
                await kid.UpdateAsync();
            }
        }
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
