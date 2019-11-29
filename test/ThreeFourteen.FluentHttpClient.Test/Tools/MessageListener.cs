using System.Net.Http;
using System.Threading.Tasks;

namespace ThreeFourteen.FluentHttpClient.Test.Tools
{
    public class MessageListener : IMessageListener
    {
        public HttpRequestMessage RequestMessage { get; private set; }
        public HttpResponseMessage ResponseMessage { get; private set; }

        public Task OnRequestMessage(HttpRequestMessage requestMessage)
        {
            RequestMessage = requestMessage;
            return Task.CompletedTask;
        }

        public Task OnResponseMessage(HttpResponseMessage responseMessage)
        {
            ResponseMessage = responseMessage;
            return Task.CompletedTask;
        }
    }
}
