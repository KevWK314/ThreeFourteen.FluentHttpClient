using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ThreeFourteen.FluentHttpClient
{
    public interface IFluentHttpClient
    {
        string Name { get; }

        FluentHttpClientConfiguration Configuration { get; }

        Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, HttpCompletionOption completionOption, CancellationToken cancellationToken);
    }

    public class FluentHttpClient : IFluentHttpClient
    {
        private readonly HttpClient _client;

        public FluentHttpClient(string name, HttpClient client)
            : this(name, client, null)
        {
        }

        public FluentHttpClient(string name, HttpClient client, FluentHttpClientConfiguration configuration)
        {
            _client = client;

            Name = name;
            Configuration = configuration ?? new FluentHttpClientConfiguration();
        }

        public string Name { get; }

        public FluentHttpClientConfiguration Configuration { get; set; }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            return _client.SendAsync(requestMessage, completionOption, cancellationToken);
        }
    }
}