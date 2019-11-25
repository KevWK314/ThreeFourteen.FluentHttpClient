using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ThreeFourteen.FluentHttpClient.Serialize
{
    public class JsonSerialization : ISerialization
    {
        public JsonSerializerSettings Settings { get; }

        public JsonSerialization()
            : this(new JsonSerializerSettings())
        {
        }

        public JsonSerialization(JsonSerializerSettings settings)
        {
            Settings = settings;
        }

        protected virtual string ContentType => Headers.Values.ContentType.Json;

        public virtual Task<HttpContent> Serialize<TRequest>(TRequest request)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(request, Settings), Encoding.UTF8, ContentType);
            return Task.FromResult(content);
        }

        public virtual async Task<TResponse> Deserialize<TResponse>(HttpContent responseContent)
        {
            var content = await responseContent.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(content, Settings);
        }
    }
}