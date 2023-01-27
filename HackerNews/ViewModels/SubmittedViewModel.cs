using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using HackerNews.Model;

namespace HackerNews.ViewModels;

public partial class SubmittedViewModel : ViewModelBase, ILazyLoadable
{
    private readonly IHackerNewsApi? _api;
    private readonly INavigation? _navigation;

    [ObservableProperty] private UserViewModel? _user;

    public SubmittedViewModel()
    {
    }

    public SubmittedViewModel(UserViewModel userViewModel)
    {
        _api = Ioc.Default.GetService<IHackerNewsApi>();
        _navigation = Ioc.Default.GetService<INavigation>();
        _user = userViewModel;
    }

    public bool IsLoaded()
    {
        return User?.Submitted is { };
    }

    public async Task LoadAsync()
    {
        if (User is { })
        {
            await User.LoadSubmittedAsync();
        }
    }

    public async Task UpdateAsync()
    {
        if (User?.Submitted is { })
        {
            foreach (var kid in User.Submitted)
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
