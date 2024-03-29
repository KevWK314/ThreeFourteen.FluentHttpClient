using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using ThreeFourteen.FluentHttpClient.Test.Model;
using ThreeFourteen.FluentHttpClient.Test.Tools;
using Xunit;

namespace ThreeFourteen.FluentHttpClient.Test
{
    public class ClientMessageListenerTests
    {
        [Fact]
        public async Task WithListener_WhenListener_ShouldUseListener()
        {
            var httpClientTester = new HttpClientTester()
                .SetResponseContent(new Person("James"));

            var listener = new MessageListener();
            var client = new FluentHttpClient("Test", httpClientTester.Client, listener);
            var response = await client
                .Get("url")
                .ExecuteAsync<Person>();

            response?.StatusCode.Should().Be(200);
            listener.RequestMessage.Should().NotBeNull();
            listener.ResponseMessage.Should().NotBeNull();
        }

        [Fact]
        public async Task WithListener_WhenListenerAutoCreated_ShouldUseListener()
        {
            var httpClientTester = new HttpClientTester()
                .SetResponseContent(new Person("James"));

            var client = new FluentHttpClientBuilder("Test", httpClientTester.Client)
                .AddMessageListener<MessageListener>()
                .Build();
            var response = await client.Get("url")
                .ExecuteAsync<Person>();

            response?.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task WithListener_WhenOnAction_ShouldCallAction()
        {
            var httpClientTester = new HttpClientTester()
                .SetResponseContent(new Person("James"));

            HttpRequestMessage requestMessage = null;
            HttpResponseMessage responseMessage = null;

            var client = new FluentHttpClientBuilder("Test", httpClientTester.Client)
                .OnRequest(r => requestMessage = r)
                .OnResponse(r => responseMessage = r)
                .Build();
            var response = await client.Get("url")
                .ExecuteAsync<Person>();

            response?.StatusCode.Should().Be(200);
            requestMessage.Should().NotBeNull();
            responseMessage.Should().NotBeNull();
        }

        [Fact]
        public async Task WithListener_WhenOnFunc_ShouldCallFunc()
        {
            var httpClientTester = new HttpClientTester()
                .SetResponseContent(new Person("James"));

            HttpRequestMessage requestMessage = null;
            HttpResponseMessage responseMessage = null;

            var client = new FluentHttpClientBuilder("Test", httpClientTester.Client)
                .OnRequest(r => { requestMessage = r; return Task.CompletedTask; })
                .OnResponse(r => { responseMessage = r; return Task.CompletedTask; })
               .Build();
            var response = await client.Get("url")
                .ExecuteAsync<Person>();

            response?.StatusCode.Should().Be(200);
            requestMessage.Should().NotBeNull();
            responseMessage.Should().NotBeNull();
        }
    }
}
