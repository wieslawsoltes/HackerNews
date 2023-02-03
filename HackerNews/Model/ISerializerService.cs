using System.IO;
using System.Threading.Tasks;

namespace HackerNews.Model;

public interface ISerializerService
{
    Task<T?> DeserializeAsync<T>(Stream stream);
}
