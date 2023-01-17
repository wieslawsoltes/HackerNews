using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace HackerNews.Services;

public class HackerNewsApiV0
{
    private const string UriPrefix = "https://hacker-news.firebaseio.com/v0/";

    public async Task<Stream> GetTopStoriesJson(HttpClient client)
    {
        var requestUri = $"{UriPrefix}/topstories.json?print=pretty";
        var response = await client.GetAsync(requestUri);
        var json = await response.Content.ReadAsStreamAsync();
        return json;
    }

    public async Task<Stream> GetItemJson(int id, HttpClient client)
    {
        var requestUri = $"{UriPrefix}/item/{id}.json?print=pretty";
        var response = await client.GetAsync(requestUri);
        var json = await response.Content.ReadAsStreamAsync();
        return json;
    }  

    public async Task<Stream> GetUserJson(string username, HttpClient client)
    {
        var requestUri = $"{UriPrefix}/user/{username}.json?print=pretty";
        var response = await client.GetAsync(requestUri);
        var json = await response.Content.ReadAsStreamAsync();
        return json;
    }
}
