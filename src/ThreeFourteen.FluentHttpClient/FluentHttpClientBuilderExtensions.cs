using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ThreeFourteen.FluentHttpClient
{
    public static class FluentHttpClientBuilderExtensions
    {
        public static FluentHttpClientBuilder AddMessageListener(this FluentHttpClientBuilder builder, IEnumerable<IMessageListener> listeners)
        {
            foreach (var listener in listeners)
            {
                builder.AddMessageListener(listener);
            }

            return builder;
        }

        public static FluentHttpClientBuilder AddMessageListener<T>(this FluentHttpClientBuilder builder) where T : IMessageListener, new()
        {
            var listener = new T();
            builder.AddMessageListener(listener);

            return builder;
        }

        public static FluentHttpClientBuilder OnRequest(this FluentHttpClientBuilder builder, Func<HttpRequestMessage, Task> onRequestMessage)
        {
            builder.AddMessageListener(new SimpleMessageListener(onRequestMessage, null));

            return builder;
        }

        public static FluentHttpClientBuilder OnResponse(this FluentHttpClientBuilder builder, Func<HttpResponseMessage, Task> onResponseMessage)
        {
            builder.AddMessageListener(new SimpleMessageListener(null, onResponseMessage));

            return builder;
        }

        public static FluentHttpClientBuilder OnRequest(this FluentHttpClientBuilder builder, Action<HttpRequestMessage> onRequestMessage)
        {
            builder.AddMessageListener(new SimpleMessageListener(onRequestMessage, null));

            return builder;
        }

        public static FluentHttpClientBuilder OnResponse(this FluentHttpClientBuilder builder, Action<HttpResponseMessage> onResponseMessage)
        {
            builder.AddMessageListener(new SimpleMessageListener(null, onResponseMessage));

            return builder;
        }
    }
}
