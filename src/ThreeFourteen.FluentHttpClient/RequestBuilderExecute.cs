using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ThreeFourteen.FluentHttpClient
{
    public partial class RequestBuilder
    {
        public virtual Task<HttpResponse> ExecuteAsync()
        {
            return ExecuteAsync(CancellationToken.None);
        }

        public virtual async Task<HttpResponse> ExecuteAsync(CancellationToken cancellationToken)
        {
            using (var requestMessage = new HttpRequestMessage(_httpMethod, GetUri()))
            {
                await ProcessRequest(requestMessage);

                using (var responseMessage = await SendAsync(requestMessage, cancellationToken))
                {
                    await ProcessResponse(responseMessage);

                    return Map(responseMessage);
                }
            }
        }

        public virtual Task<HttpResponse<TResponse>> ExecuteAsync<TResponse>()
        {
            return ExecuteAsync<TResponse>(CancellationToken.None);
        }

        public virtual async Task<HttpResponse<TResponse>> ExecuteAsync<TResponse>(CancellationToken cancellationToken)
        {
            using (var requestMessage = new HttpRequestMessage(_httpMethod, GetUri()))
            {
                await ProcessRequest(requestMessage);

                using (var responseMessage = await SendAsync(requestMessage, cancellationToken))
                {
                    await ProcessResponse(responseMessage);

                    var result = await Deserialize<TResponse>(responseMessage.Content);

                    return Map(responseMessage, result);
                }
            }
        }

        public virtual Task<HttpResponse<TResponse>> ExecuteAsync<TRequest, TResponse>(TRequest request)
        {
            return ExecuteAsync<TRequest, TResponse>(request, CancellationToken.None);
        }

        public async Task<HttpResponse<TResponse>> ExecuteAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
        {
            using (var requestMessage = new HttpRequestMessage(_httpMethod, GetUri()))
            {
                requestMessage.Content = await Serialize(request);

                await ProcessRequest(requestMessage);

                using (var responseMessage = await SendAsync(requestMessage, cancellationToken))
                {
                    await ProcessResponse(responseMessage);

                    var result = await Deserialize<TResponse>(responseMessage.Content);

                    return Map(responseMessage, result);
                }
            }
        }

        private static HttpResponse Map(HttpResponseMessage responseMessage)
        {
            var response = new HttpResponse();
            UpdateResponse(responseMessage, response);

            return response;
        }

        private static HttpResponse<T> Map<T>(HttpResponseMessage responseMessage, T result)
        {
            var response = new HttpResponse<T> { Result = result };
            UpdateResponse(responseMessage, response);

            return response;
        }

        private static void UpdateResponse(HttpResponseMessage responseMessage, HttpResponse httpResponse)
        {
            httpResponse.StatusCode = (int)responseMessage.StatusCode;
            httpResponse.ReasonPhrase = responseMessage.ReasonPhrase;
            httpResponse.IsSuccessStatusCode = responseMessage.IsSuccessStatusCode;
            httpResponse.ResponseHeaders = responseMessage.Headers?
                .Select(h => new KeyValuePair<string, string[]>(h.Key, h.Value.ToArray()))
                .ToArray();
        }
    }
}
