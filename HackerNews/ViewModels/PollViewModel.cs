using System.Threading.Tasks;
using HackerNews.Model;
using HackerNews.Services;

namespace HackerNews.ViewModels;

public partial class PollViewModel : ViewModelBase, ILazyLoadable
{
    private readonly HackerNewsApiV0? _api;
    private readonly INavigation? _navigation;
    private readonly ItemViewModel? _itemViewModel;

    public PollViewModel()
    {
    }

    public PollViewModel(HackerNewsApiV0 api, INavigation navigation, ItemViewModel itemViewModel)
    {
        _api = api;
        _navigation = navigation;
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
