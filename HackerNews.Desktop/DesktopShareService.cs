using System.Threading.Tasks;
using HackerNews.Model;

namespace HackerNews.Desktop;

public class DesktopShareService : IShareService
{
    public async Task ShareTextAsync(string title, string text, string uri)
    {
        // TODO: 
        await Task.Yield();
    }
}
