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
        _items.Add(lazyLoadable);

        CanGoBack = _items.Count > 1;

        await lazyLoadable.LoadAsync();
    }

    public async Task BackAsync()
    {
        if (_items.Count > 1)
        {
            var lazyLoadable = _items[_items.Count - 1];

            _items.Remove(lazyLoadable);

            CanGoBack = _items.Count > 1;

            await lazyLoadable.BackAsync();
        }
    }
}
