using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using HackerNews.Model;

namespace HackerNews.ViewModels;

public partial class PollViewModel : ViewModelBase, ILazyLoadable
{
    private readonly IHackerNewsApi? _api;
    private readonly INavigation? _navigation;
    private readonly ItemViewModel? _itemViewModel;

    public PollViewModel()
    {
    }

    public PollViewModel(ItemViewModel itemViewModel)
    {
        _api = Ioc.Default.GetService<IHackerNewsApi>();
        _navigation = Ioc.Default.GetService<INavigation>();
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
        // TODO:
        return await Task.FromResult(false);
    }
}
