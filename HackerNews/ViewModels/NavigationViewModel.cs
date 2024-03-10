using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using HackerNews.Model;

namespace HackerNews.ViewModels;

public partial class NavigationViewModel : ViewModelBase, INavigationService
{
    [ObservableProperty] private LazyLoadableList _items;
    [ObservableProperty] private bool _canGoBack;
    [ObservableProperty] private bool _isHeaderVisible;

    public NavigationViewModel()
    {
        _items = new LazyLoadableList();
    }

    public async Task NavigateAsync(ILazyLoadable lazyLoadable)
    {
        if (Items.Count >= 1)
        {
            var currentLazyLoadable = Items[^1];
            currentLazyLoadable.IsVisible = false;
        }

        Items.Add(lazyLoadable);
        lazyLoadable.IsVisible = true;

        CanGoBack = Items.Count > 1;
        
        IsHeaderVisible = Items.Count <= 1;

        await lazyLoadable.LoadAsync();
    }

    public async Task Clear()
    {
        Items.Clear();

        CanGoBack = false;
        IsHeaderVisible = true;

        await Task.Yield();
    }

    public async Task<bool> BackAsync()
    {
        if (Items.Count > 1)
        {
            var currentLazyLoadable = Items[^1];
            currentLazyLoadable.IsVisible = false;

            if (Items.Count - 1 >= 1)
            {
                var nextLazyLoadable = Items[^2];
                nextLazyLoadable.IsVisible = true;
            }

            CanGoBack = Items.Count - 1 > 1;

            IsHeaderVisible = Items.Count - 1 <= 1;

            await Task.Delay(400);

            Items.Remove(currentLazyLoadable);

            return await Task.FromResult(true);
        }

        IsHeaderVisible = true;

        return await Task.FromResult(false);
    }
}
