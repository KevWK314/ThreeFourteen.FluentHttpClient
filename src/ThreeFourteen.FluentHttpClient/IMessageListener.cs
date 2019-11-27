using System.Net.Http;
using System.Threading.Tasks;

namespace ThreeFourteen.FluentHttpClient
{
    public interface IMessageListener
    {
        Task OnRequestMessage(HttpRequestMessage requestMessage);
        Task OnResponseMessage(HttpResponseMessage responseMessage);
    }
}
