using System;
using System.Collections.Generic;
using System.Net.Http;

namespace ThreeFourteen.FluentHttpClient
{
    public sealed class FluentHttpClientBuilder
    {
        private readonly string _name;
        private readonly HttpClient _client;
        private FluentHttpClientOptions _options = new FluentHttpClientOptions();
        private List<IMessageListener> _listeners = new List<IMessageListener>();

        public FluentHttpClientBuilder(string name, HttpClient client)
        {
            _name = name;
            _client = client;
        }

        public FluentHttpClientBuilder Configure(Action<FluentHttpClientOptions> configure)
        {
            configure?.Invoke(_options);
            return this;
        }

        public FluentHttpClientBuilder AddMessageListener(IMessageListener listener)
        {
            _listeners.Add(listener);
            return this;
        }

        public FluentHttpClient Build()
        {
            return new FluentHttpClient(_name, _client, _options, _listeners.ToArray());
        }
    }
}
