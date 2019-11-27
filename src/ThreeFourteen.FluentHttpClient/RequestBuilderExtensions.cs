using System;
using System.Net.Http;
using System.Threading.Tasks;
using ThreeFourteen.FluentHttpClient.Serialize;

namespace ThreeFourteen.FluentHttpClient
{
    public static class RequestBuilderExtensions
    {
        public static RequestBuilder OnRequest(this RequestBuilder requestBuilder, Action<HttpRequestMessage> onRequestMessage)
        {
            requestBuilder.AddMessageListener(new SimpleMessageListener(onRequestMessage, null));

            return requestBuilder;
        }

        public static RequestBuilder OnResponse(this RequestBuilder requestBuilder, Action<HttpResponseMessage> onResponseMessage)
        {
            requestBuilder.AddMessageListener(new SimpleMessageListener(null, onResponseMessage));

            return requestBuilder;
        }

        public static RequestBuilder OnRequest(this RequestBuilder requestBuilder, Func<HttpRequestMessage, Task> onRequestMessage)
        {
            requestBuilder.AddMessageListener(new SimpleMessageListener(onRequestMessage, null));

            return requestBuilder;
        }

        public static RequestBuilder OnResponse(this RequestBuilder requestBuilder, Func<HttpResponseMessage, Task> onResponseMessage)
        {
            requestBuilder.AddMessageListener(new SimpleMessageListener(null, onResponseMessage));

            return requestBuilder;
        }

        public static RequestBuilder WithHeader(this RequestBuilder requestBuilder, string name, params string[] values)
        {
            requestBuilder.OnRequest(r => r.Headers.Add(name, values));

            return requestBuilder;
        }

        public static RequestBuilder WithSerializer(this RequestBuilder requestBuilder, ISerialization serialization)
        {
            requestBuilder.UpdateConfiguration(c => c.Serialization = serialization);

            return requestBuilder;
        }
    }
}