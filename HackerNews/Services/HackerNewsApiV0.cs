using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HackerNews.Services;

public class HackerNewsApiV0
{
    private const string UriPrefix = "https://hacker-news.firebaseio.com/v0/";
    private HttpClient _client;
    private JsonSerializerOptions _options;

    public HackerNewsApiV0()
    {
        _client = new HttpClient();

        _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<T?> DeserializeAsync<T>(Stream stream)
    {
        return await JsonSerializer.DeserializeAsync<T>(stream, _options);
    }

    public async Task<Stream> GetTopStoriesJson()
    {
        var requestUri = $"{UriPrefix}/topstories.json?print=pretty";
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
