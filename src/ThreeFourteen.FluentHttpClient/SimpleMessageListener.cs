using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ThreeFourteen.FluentHttpClient
{
    public class SimpleMessageListener : IMessageListener
    {
        private readonly Func<HttpRequestMessage, Task> _onRequestMessage;
        private readonly Func<HttpResponseMessage, Task> _onResponseMessage;

        public SimpleMessageListener(Action<HttpRequestMessage> onRequestMessage, Action<HttpResponseMessage> onResponseMessage)
            : this(Convert(onRequestMessage), Convert(onResponseMessage))
        {
        }

        public SimpleMessageListener(Func<HttpRequestMessage, Task> onRequestMessage, Func<HttpResponseMessage, Task> onResponseMessage)
        {
            _onRequestMessage = onRequestMessage;
            _onResponseMessage = onResponseMessage;
        }

        public Task OnRequestMessage(HttpRequestMessage requestMessage)
        {
            return _onRequestMessage?.Invoke(requestMessage) ?? Task.CompletedTask; 
        }

        public Task OnResponseMessage(HttpResponseMessage responseMessage)
        {
            return _onResponseMessage?.Invoke(responseMessage) ?? Task.CompletedTask;
        }

        private static Func<HttpRequestMessage, Task> Convert(Action<HttpRequestMessage> onRequestMessage)
        {
            return x =>
                {
                    onRequestMessage?.Invoke(x);
                    return Task.CompletedTask;
                };
        }

        private static Func<HttpResponseMessage, Task> Convert(Action<HttpResponseMessage> onResponseMessage)
        {
            return x =>
                {
                    onResponseMessage?.Invoke(x);
                    return Task.CompletedTask;
                };
        }
    }
}
