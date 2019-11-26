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

        public virtual async Task<HttpContent> Serialize<TRequest>(TRequest request)
        {
            using (var memoryStream = new MemoryStream())
            using (StreamWriter sw = new StreamWriter(memoryStream))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                var streamContent = new StreamContent(memoryStream);
                streamContent.Headers.Add(Headers.Key.ContentType, Headers.Values.ContentType.Json);

                _serializer.Serialize(writer, request);
                writer.Flush();
                memoryStream.Position = 0;

                await streamContent.LoadIntoBufferAsync();
                return streamContent;
            }
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