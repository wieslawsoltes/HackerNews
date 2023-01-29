using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HackerNews.Model;

public interface INavigationService
{
    ObservableCollection<ILazyLoadable> Items { get; set; }
    bool CanGoBack { get; set; }
    Task NavigateAsync(ILazyLoadable lazyLoadable);
    Task Clear();
    Task<bool> BackAsync();
}
