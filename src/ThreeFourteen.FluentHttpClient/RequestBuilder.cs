using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ThreeFourteen.FluentHttpClient.Serialize;

namespace ThreeFourteen.FluentHttpClient
{
    public partial class RequestBuilder
    {
        protected readonly IFluentHttpClient Client;
        protected readonly string Uri;
        protected readonly HttpMethod HttpMethod;

        protected FluentHttpClientConfiguration Configuration = new FluentHttpClientConfiguration();
        protected List<Action<HttpRequestMessage>> RequestMessageActions = new List<Action<HttpRequestMessage>>();
        protected List<Action<HttpResponseMessage>> ResponseMessageActions = new List<Action<HttpResponseMessage>>();

        public RequestBuilder(IFluentHttpClient client, string uri, HttpMethod httpMethod)
        {
            HttpMethod = httpMethod;
            Client = client;
            Uri = uri;
        }

        public RequestBuilder UpdateConfiguration(Action<FluentHttpClientConfiguration> update)
        {
            update?.Invoke(Configuration);
            return this;
        }

        public RequestBuilder OnRequest(Action<HttpRequestMessage> action)
        {
            RequestMessageActions.Add(action);
            return this;
        }

        public RequestBuilder OnResponse(Action<HttpResponseMessage> action)
        {
            ResponseMessageActions.Add(action);
            return this;
        }

        protected Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage)
        {
            return Client.SendAsync(
                requestMessage,
                Configuration.HttpCompletionOption ?? Client.Configuration.HttpCompletionOption ?? HttpCompletionOption.ResponseContentRead,
                Configuration.CancellationToken ?? Client.Configuration.CancellationToken ?? CancellationToken.None);
        }

        protected void ProcessRequest(HttpRequestMessage requestMessage)
        {
            RequestMessageActions.ForEach(configure => configure?.Invoke(requestMessage));
        }

        protected void ProcessResponse(HttpResponseMessage responseMessage)
        {
            ResponseMessageActions.ForEach(configure => configure?.Invoke(responseMessage));

            if (Configuration.EnsureSuccessStatusCode ?? Client.Configuration.EnsureSuccessStatusCode ?? true)
                responseMessage.EnsureSuccessStatusCode();
        }

        protected Task<HttpContent> Serialize<T>(T data)
        {
            var serializer = Configuration.Serialization ??
                             Client.Configuration.Serialization ??
                             Serialization.Default;
            return serializer.Serialize(data);
        }

        protected Task<T> Deserialize<T>(HttpContent content)
        {
            var serializer = Configuration.Serialization ??
                             Client.Configuration.Serialization ??
                             Serialization.Default;
            return serializer.Deserialize<T>(content);
        }
    }
}
