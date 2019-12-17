using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ThreeFourteen.FluentHttpClient.Serialize
{
    public class JsonSerialization : ISerialization
    {
        private readonly JsonSerializer _serializer;

        public JsonSerialization()
            : this(new JsonSerializerSettings())
        {
        }

        public JsonSerialization(JsonSerializerSettings settings)
        {
            _serializer = JsonSerializer.Create(settings);
        }

        public virtual Task<HttpContent> Serialize<TRequest>(TRequest request)
        {
            HttpContent content = new JsonHttpContent<TRequest>(request, _serializer);
            content.Headers.Add(Headers.Key.ContentType, Headers.Values.ContentType.Json);

            return Task.FromResult(content);
        }

        public virtual async Task<TResponse> Deserialize<TResponse>(HttpContent responseContent)
        {
            using (var contentStream = await responseContent.ReadAsStreamAsync())
            using (var streamReader = new StreamReader(contentStream))
            {
                using (JsonReader reader = new JsonTextReader(streamReader))
                {
                    return _serializer.Deserialize<TResponse>(reader);
                }
            }
        }
    }
}