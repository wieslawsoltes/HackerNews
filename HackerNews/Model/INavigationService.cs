using System.Threading.Tasks;

namespace HackerNews.Model;

public interface INavigationService
{
    LazyLoadableList Items { get; set; }
    bool CanGoBack { get; set; }
    bool IsHeaderVisible { get; set; }
    Task NavigateAsync(ILazyLoadable lazyLoadable);
    Task Clear();
    Task<bool> BackAsync();
}
