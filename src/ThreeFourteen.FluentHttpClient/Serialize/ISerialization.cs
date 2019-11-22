using System.Net.Http;
using System.Threading.Tasks;

namespace FluentHttpClient.Serialize
{
    public interface ISerialization
    {
        Task<HttpContent> Serialize<TRequest>(TRequest request);
        Task<TResponse> Deserialize<TResponse>(HttpContent responseContent);
    }
}