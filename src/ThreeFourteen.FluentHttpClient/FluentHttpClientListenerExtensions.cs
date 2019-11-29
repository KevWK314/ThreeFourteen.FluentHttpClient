using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ThreeFourteen.FluentHttpClient
{
    public static class FluentHttpClientListenerExtensions
    {
        public static IFluentHttpClient WithListener(this IFluentHttpClient fluentHttpClient, IMessageListener messageListener)
        {
            fluentHttpClient.Listeners.Add(messageListener);

            return fluentHttpClient;
        }

        public static IFluentHttpClient WithListener<T>(this IFluentHttpClient fluentHttpClient) where T : IMessageListener, new()
        {
            var listener = new T();
            fluentHttpClient.Listeners.Add(listener);

            return fluentHttpClient;
        }

        public static IFluentHttpClient OnRequest(this IFluentHttpClient fluentHttpClient, Func<HttpRequestMessage, Task> onRequestMessage)
        {
            fluentHttpClient.WithListener(new SimpleMessageListener(onRequestMessage, null));

            return fluentHttpClient;
        }

        public static IFluentHttpClient OnResponse(this IFluentHttpClient fluentHttpClient, Func<HttpResponseMessage, Task> onResponseMessage)
        {
            fluentHttpClient.WithListener(new SimpleMessageListener(null, onResponseMessage));

            return fluentHttpClient;
        }

        public static IFluentHttpClient OnRequest(this IFluentHttpClient fluentHttpClient, Action<HttpRequestMessage> onRequestMessage)
        {
            fluentHttpClient.WithListener(new SimpleMessageListener(onRequestMessage, null));

            return fluentHttpClient;
        }

        public static IFluentHttpClient OnResponse(this IFluentHttpClient fluentHttpClient, Action<HttpResponseMessage> onResponseMessage)
        {
            fluentHttpClient.WithListener(new SimpleMessageListener(null, onResponseMessage));

            return fluentHttpClient;
        }
    }
}
