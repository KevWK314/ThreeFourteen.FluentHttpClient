using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ThreeFourteen.FluentHttpClient
{
    public interface IFluentHttpClient
    {
        string Name { get; }

        FluentHttpClientConfiguration Configuration { get; }

        IList<IMessageListener> Listeners { get; }

        Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, HttpCompletionOption completionOption, CancellationToken cancellationToken);
    }
}