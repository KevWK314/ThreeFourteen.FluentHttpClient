using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ThreeFourteen.FluentHttpClient.Serialize;

namespace ThreeFourteen.FluentHttpClient
{
    public partial class RequestBuilder
    {
        private readonly IFluentHttpClient _client;
        private readonly string _uri;
        private readonly HttpMethod _httpMethod;

        private readonly FluentHttpClientConfiguration _configuration = new FluentHttpClientConfiguration();
        private readonly List<IMessageListener> _listeners = new List<IMessageListener>();
        private readonly Dictionary<string, string> _parameters = new Dictionary<string, string>();

        public RequestBuilder(IFluentHttpClient client, string uri, HttpMethod httpMethod)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            _httpMethod = httpMethod;
            _client = client;
            _uri = uri;
        }

        public RequestBuilder UpdateConfiguration(Action<FluentHttpClientConfiguration> update)
        {
            update?.Invoke(_configuration);
            return this;
        }

        internal RequestBuilder AddMessageListener(IMessageListener messageListener)
        {
            if (messageListener == null) throw new ArgumentNullException(nameof(messageListener));

            _listeners.Add(messageListener);
            return this;
        }

        public RequestBuilder AddQueryParameter(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException(nameof(key));
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException(nameof(value));

            _parameters[WebUtility.UrlEncode(key) ?? string.Empty] = WebUtility.UrlEncode(value);

            return this;
        }

        protected Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage)
        {
            return _client.SendAsync(
                requestMessage,
                _configuration.HttpCompletionOption ?? _client.Configuration.HttpCompletionOption ?? HttpCompletionOption.ResponseContentRead,
                _configuration.CancellationToken ?? _client.Configuration.CancellationToken ?? CancellationToken.None);
        }

        protected async Task ProcessRequest(HttpRequestMessage requestMessage)
        {
            foreach (var listener in _client.Listeners.Concat(_listeners))
            {
                await listener.OnRequestMessage(requestMessage);
            }
        }

        protected async Task ProcessResponse(HttpResponseMessage responseMessage)
        {
            foreach (var listener in _client.Listeners.Concat(_listeners))
            {
                await listener.OnResponseMessage(responseMessage);
            }

            if (_configuration.EnsureSuccessStatusCode ?? _client.Configuration.EnsureSuccessStatusCode ?? true)
                responseMessage.EnsureSuccessStatusCode();
        }

        protected Task<HttpContent> Serialize<T>(T data)
        {
            var serializer = _configuration.Serialization ??
                             _client.Configuration.Serialization ??
                             Serialization.Default;
            return serializer.Serialize(data);
        }

        protected Task<T> Deserialize<T>(HttpContent content)
        {
            var serializer = _configuration.Serialization ??
                             _client.Configuration.Serialization ??
                             Serialization.Default;
            return serializer.Deserialize<T>(content);
        }

        protected string GetUri()
        {
            if (_parameters.Count == 0)
                return _uri;

            var parameters = string.Join("&",
                _parameters.Select(p => $"{p.Key}={p.Value}"));

            return $"{_uri}?{parameters}";
        }
    }
}
