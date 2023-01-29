using System.IO;
using System.Threading.Tasks;

namespace HackerNews.Model;

public interface IHackerNewsService
{
    Task<T?> DeserializeAsync<T>(Stream stream);
    Task<Stream> GetStoriesJson(string storiesFeed);
    Task<Stream> GetItemJson(int id);
    Task<Stream> GetUserJson(string username);
}
