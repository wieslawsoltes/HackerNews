using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using HackerNews.Model;

namespace HackerNews.ViewModels;

public partial class PollViewModel : ViewModelBase, ILazyLoadable
{
    private readonly ItemViewModel? _itemViewModel;

    public PollViewModel()
    {
    }

    public PollViewModel(ItemViewModel itemViewModel)
    {
        _itemViewModel = itemViewModel;
    }

    public bool IsLoaded()
    {
        return _itemViewModel?.Parts is { };
    }

    public async Task LoadAsync()
    {
        if (_itemViewModel is { })
        {
            await _itemViewModel.LoadKPartsAsync();
        }
    }

    public async Task UpdateAsync()
    {
        if (_itemViewModel?.Parts is { })
        {
            foreach (var part in _itemViewModel.Parts)
            {
                await part.UpdateAsync();
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
