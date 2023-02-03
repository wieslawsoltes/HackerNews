using System.IO;
using System.Threading.Tasks;

namespace HackerNews.Model;

public interface IHackerNewsService
{
    Task<Stream> GetStoriesJson(string storiesFeed);
    Task<Stream> GetItemJson(int id);
    Task<Stream> GetUserJson(string username);
}
