using System.Threading.Tasks;

namespace HackerNews.Model;

public interface IShareService
{
    Task ShareTextAsync(string title, string text, string uri);
}
