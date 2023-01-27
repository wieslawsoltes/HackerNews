using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using HackerNews.Model;

namespace HackerNews.ViewModels;

public partial class NavigationViewModel : ViewModelBase, INavigation
{
    [ObservableProperty] private ObservableCollection<ILazyLoadable> _items;
    [ObservableProperty] private bool _canGoBack;

    public NavigationViewModel()
    {
        _items = new ObservableCollection<ILazyLoadable>();
    }

    public async Task NavigateAsync(ILazyLoadable lazyLoadable)
    {
        Items.Add(lazyLoadable);

        CanGoBack = Items.Count > 1;

        await lazyLoadable.LoadAsync();
    }

    public async Task Clear()
    {
        Items.Clear();

        CanGoBack = false;

        await Task.Yield();
    }

    public async Task<bool> BackAsync()
    {
        if (Items.Count > 1)
        {
            var lazyLoadable = Items[Items.Count - 1];

            Items.Remove(lazyLoadable);

            CanGoBack = Items.Count > 1;

            return await Task.FromResult(true);
        }

        return await Task.FromResult(false);
    }
}
