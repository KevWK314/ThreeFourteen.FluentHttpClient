using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ThreeFourteen.FluentHttpClient.Test.Tools
{
    public class HttpClientTester : HttpMessageHandler
    {
        private readonly HttpResponseMessage _responseMessage = new HttpResponseMessage(HttpStatusCode.OK);

        public HttpRequestMessage RequestMessage { get; private set; }

        public HttpClient Client { get; }

        public HttpClientTester()
        {
            Client = new HttpClient(this) { BaseAddress = new Uri("http://address") };
        }

        public HttpClientTester SetResponseStatusCode(HttpStatusCode code)
        {
            _responseMessage.StatusCode = code;
            return this;
        }

        public HttpClientTester SetResponseContent<T>(T content)
        {
            _responseMessage.Content = new StringContent(JsonConvert.SerializeObject(content));
            return this;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            RequestMessage = request;
            return await Task.FromResult(_responseMessage);
        }
    }
}