using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using HackerNews.Model;

namespace HackerNews.Services;

public class HackerNewsServiceV0 : IHackerNewsService
{
    private const string UriPrefix = "https://hacker-news.firebaseio.com/v0/";
    private readonly HttpClient _client;

    public HackerNewsServiceV0()
    {
        _client = new HttpClient();
    }

    public async Task<Stream> GetStoriesJson(string storiesFeed)
    {
        var requestUri = $"{UriPrefix}/{storiesFeed}.json?print=pretty";
        var response = await _client.GetAsync(requestUri);
        var json = await response.Content.ReadAsStreamAsync();
        return json;
    }

    public async Task<Stream> GetItemJson(int id)
    {
        var requestUri = $"{UriPrefix}/item/{id}.json?print=pretty";
        var response = await _client.GetAsync(requestUri);
        var json = await response.Content.ReadAsStreamAsync();
        return json;
    }  

    public async Task<Stream> GetUserJson(string username)
    {
        var requestUri = $"{UriPrefix}/user/{username}.json?print=pretty";
        var response = await _client.GetAsync(requestUri);
        var json = await response.Content.ReadAsStreamAsync();
        return json;
    }
}
