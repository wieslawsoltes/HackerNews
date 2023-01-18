using System.Threading.Tasks;

namespace HackerNews.Model;

public interface ILazyLoadable
{
    bool IsLoaded();
    Task Load();
    Task Update();
    Task Back();
}
