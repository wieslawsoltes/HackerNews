using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using HackerNews.Model;

namespace HackerNews.ViewModels;

public partial class SettingsViewModel : ViewModelBase, ILazyLoadable
{
    [ObservableProperty] private bool _isVisible;

    public SettingsViewModel()
    {
    }

    public bool IsLoaded()
    {
        return false;
    }

    public async Task LoadAsync()
    {
        // TODO:
        await Task.Yield();
    }

    public async Task UpdateAsync()
    {
        // TODO:
        await Task.Yield();
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
