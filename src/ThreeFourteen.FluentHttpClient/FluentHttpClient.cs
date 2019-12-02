using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ThreeFourteen.FluentHttpClient
{
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

        public IList<IMessageListener> Listeners { get; } = new List<IMessageListener>();

        public virtual Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            return _client.SendAsync(requestMessage, completionOption, cancellationToken);
        }
    }
}