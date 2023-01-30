using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using HackerNews.Model;

namespace HackerNews.ViewModels;

public partial class PollViewModel : ViewModelBase, ILazyLoadable
{
    [ObservableProperty] private bool _isVisible;
    [ObservableProperty] private ItemViewModel? _item;

    public PollViewModel()
    {
    }

    public PollViewModel(ItemViewModel item)
    {
        _item = item;
    }

    public bool IsLoaded()
    {
        return Item?.Parts is { };
    }

    public async Task LoadAsync()
    {
        if (Item is { })
        {
            await Item.LoadKPartsAsync();
        }
    }

    public async Task UpdateAsync()
    {
        if (Item?.Parts is { })
        {
            foreach (var part in Item.Parts)
            {
                await part.UpdateAsync();
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
