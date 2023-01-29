using System.Threading.Tasks;

namespace HackerNews.Model;

public interface IBrowserService
{
    Task OpenBrowserAsync(System.Uri uri, bool external = false);
}
