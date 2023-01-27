using System.Threading.Tasks;
using HackerNews.Model;
using HackerNews.Services;

namespace HackerNews.ViewModels;

public partial class SubmittedViewModel : ViewModelBase, ILazyLoadable
{
    private readonly IHackerNewsApi? _api;
    private readonly INavigation? _navigation;
    private readonly UserViewModel? _userViewModel;

    public SubmittedViewModel()
    {
    }

    public SubmittedViewModel(IHackerNewsApi api, INavigation navigation, UserViewModel userViewModel)
    {
        _api = api;
        _navigation = navigation;
        _userViewModel = userViewModel;
    }

    public bool IsLoaded()
    {
        return _userViewModel?.Submitted is { };
    }

    public async Task LoadAsync()
    {
        if (_userViewModel is { })
        {
            await _userViewModel.LoadSubmittedAsync();
        }
    }

    public async Task UpdateAsync()
    {
        if (_userViewModel?.Submitted is { })
        {
            foreach (var kid in _userViewModel.Submitted)
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
