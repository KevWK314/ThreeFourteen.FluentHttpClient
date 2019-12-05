using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ThreeFourteen.FluentHttpClient
{
    public class FluentHttpClient
    {
        private static readonly IMessageListener[] NoListeners = new IMessageListener[0];
        private readonly HttpClient _client;

        public FluentHttpClient(string name, HttpClient client)
            : this(name, client, null, NoListeners)
        {
        }

        public FluentHttpClient(string name, HttpClient client, FluentHttpClientOptions options)
            : this(name, client, options, NoListeners)
        {
        }

        public FluentHttpClient(string name, HttpClient client, params IMessageListener[] listeners)
            : this(name, client, null, listeners)
        {
        }

        public FluentHttpClient(
            string name,
            HttpClient client,
            FluentHttpClientOptions options,
            params IMessageListener[] messageListeners)
        {
            _client = client;

            Name = name;
            Options = options ?? new FluentHttpClientOptions();
            Listeners.AddRange(messageListeners ?? NoListeners);
        }

        public string Name { get; }

        protected virtual FluentHttpClientOptions Options { get; }

        protected virtual List<IMessageListener> Listeners { get; } = new List<IMessageListener>();

        public virtual Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            return _client.SendAsync(requestMessage, completionOption, cancellationToken);
        }

        internal FluentHttpClientOptions GetOptions()
        {
            return Options;
        }

        internal IEnumerable<IMessageListener> GetListeners()
        {
            return Listeners.AsEnumerable();
        }
    }
}