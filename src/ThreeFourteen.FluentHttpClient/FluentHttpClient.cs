using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FluentHttpClient
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

        internal FluentHttpClient(string name, HttpClient client, FluentHttpClientConfiguration configuration)
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

        public static IFluentHttpClient Create(string name, HttpClient client)
        {
            return new FluentHttpClient(name, client, null);
        }

        public static IFluentHttpClient Create(string name, HttpClient client, FluentHttpClientConfiguration configuration)
        {
            return new FluentHttpClient(name, client, configuration);
        }
    }
}