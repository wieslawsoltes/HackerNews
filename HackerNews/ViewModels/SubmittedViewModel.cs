using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using HackerNews.Model;

namespace HackerNews.ViewModels;

public partial class SubmittedViewModel : ViewModelBase, ILazyLoadable
{
    [ObservableProperty] private bool _isVisible;
    [ObservableProperty] private UserViewModel? _user;

    public SubmittedViewModel()
    {
    }

    public SubmittedViewModel(UserViewModel userViewModel)
    {
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
        var navigation = Ioc.Default.GetService<INavigationService>();
        if (navigation is { })
        {
            return await navigation.BackAsync();
        }

        return await Task.FromResult(false);
    }
}
