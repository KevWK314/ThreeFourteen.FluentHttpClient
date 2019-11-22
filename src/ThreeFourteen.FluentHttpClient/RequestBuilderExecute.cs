using System.Net.Http;
using System.Threading.Tasks;

namespace FluentHttpClient
{
    public partial class RequestBuilder
    {
        public virtual async Task ExecuteAsync()
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod, Uri))
            {
                ProcessRequest(requestMessage);

                using (var responseMessage = await SendAsync(requestMessage))
                {
                    ProcessResponse(responseMessage);
                }
            }
        }

        public virtual async Task<TResponse> ExecuteAsync<TResponse>()
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod, Uri))
            {
                ProcessRequest(requestMessage);

                using (var responseMessage = await SendAsync(requestMessage))
                {
                    ProcessResponse(responseMessage);

                    return await Deserialize<TResponse>(responseMessage.Content);
                }
            }
        }

        public async Task<TResponse> ExecuteAsync<TRequest, TResponse>(TRequest request)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod, Uri))
            {
                requestMessage.Content = await Serialize(request);

                ProcessRequest(requestMessage);

                using (var responseMessage = await SendAsync(requestMessage))
                {
                    ProcessResponse(responseMessage);

                    return await Deserialize<TResponse>(responseMessage.Content);
                }
            }
        }
    }
}
