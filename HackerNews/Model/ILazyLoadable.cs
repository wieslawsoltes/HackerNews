using System.Threading.Tasks;

namespace HackerNews.Model;

public interface ILazyLoadable
{
    bool IsLoaded();
    Task LoadAsync();
    Task UpdateAsync();
    Task<bool> BackAsync();
}
