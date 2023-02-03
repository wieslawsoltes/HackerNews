using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using HackerNews.Model;

namespace HackerNews.Services;

public class JsonSerializerService : ISerializerService
{
    private readonly JsonSerializerOptions _options;

    public JsonSerializerService()
    {
        _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<T?> DeserializeAsync<T>(Stream stream)
    {
        return await JsonSerializer.DeserializeAsync<T>(stream, _options);
    }
}
