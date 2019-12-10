using System;
using System.Threading.Tasks;
using FluentAssertions;
using ThreeFourteen.FluentHttpClient.Test.Model;
using NSubstitute;
using Xunit;
using ThreeFourteen.FluentHttpClient.Serialize;
using System.Net.Http;
using System.Threading;
using System.Net;
using ThreeFourteen.FluentHttpClient.Test.Tools;

namespace ThreeFourteen.FluentHttpClient.Test
{
    public class ClientOptionsTests
    {
        private readonly ISerialization _serialization;
        private readonly FluentHttpClientOptions _options;
        private readonly HttpClientTester _testHttpClient;
        private readonly TestClient _client;

        public ClientOptionsTests()
        {
            _serialization = Substitute.For<ISerialization>();
            _options = new FluentHttpClientOptions();
            _testHttpClient = new HttpClientTester();
            _client = new TestClient("Test", _testHttpClient.Client, _options);
        }

        [Fact]
        public async Task Config_WhenCompletionOptionsNotSet_ShouldDefault()
        {
            var response = await _client.Get("url").ExecuteAsync();

            response?.StatusCode.Should().Be(200);

            _client.CompletionOption.Should().Be(HttpCompletionOption.ResponseContentRead);
        }

        [Fact]
        public async Task ClientConfig_WhenCompletionOptionsSet_ShouldBeUsedWhenSending()
        {
            _options.HttpCompletionOption = HttpCompletionOption.ResponseHeadersRead;
            var response = await _client.Get("url").ExecuteAsync();

            response?.StatusCode.Should().Be(200);

            _client.CompletionOption.Should().Be(HttpCompletionOption.ResponseHeadersRead);
        }

        [Fact]
        public async Task RequestConfig_WhenCompletionOptionsSet_ShouldBeUsedWhenSending()
        {
            var response = await _client.Get("url")
                .Configure(c => c.HttpCompletionOption = HttpCompletionOption.ResponseHeadersRead)
                .ExecuteAsync();

            response?.StatusCode.Should().Be(200);

            _client.CompletionOption.Should().Be(HttpCompletionOption.ResponseHeadersRead);
        }

        [Fact]
        public async Task ClientConfig_WhenSerialisationSet_ShouldBeUsedWhenSending()
        {
            _options.Serialization = _serialization;
            _serialization.Deserialize<Person>(Arg.Any<HttpContent>()).Returns(Task.FromResult(new Person("Lisa")));

            var response = await _client.Get("url").ExecuteAsync<Person>();

            response?.StatusCode.Should().Be(200);
            response.Result?.Name.Should().Be("Lisa");
        }

        [Fact]
        public async Task RequestConfig_WhenSerialisationSet_ShouldBeUsedWhenSending()
        {
            _serialization.Deserialize<Person>(Arg.Any<HttpContent>()).Returns(Task.FromResult(new Person("Lisa")));

            var response = await _client.Get("url")
                .Configure(c => c.Serialization = _serialization)
                .ExecuteAsync<Person>();

            response?.StatusCode.Should().Be(200);
            response.Result?.Name.Should().Be("Lisa");
        }

        [Fact]
        public async Task Config_WhenEnsureSuccessNotSet_ShouldDefault()
        {
            _testHttpClient.SetResponseStatusCode(HttpStatusCode.InternalServerError);

            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => _client.Get("url").ExecuteAsync());
        }

        [Fact]
        public async Task ClientConfig_WhenEnsureSuccessSet_ShouldUseInResponse()
        {
            _testHttpClient.SetResponseStatusCode(HttpStatusCode.InternalServerError);
            _options.EnsureSuccessStatusCode = false;

            var result = await _client.Get("url").ExecuteAsync();

            result.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task RequestConfig_WhenEnsureSuccessSet_ShouldUseInResponse()
        {
            _testHttpClient.SetResponseStatusCode(HttpStatusCode.InternalServerError);

            var result = await _client.Get("url")
                .Configure(c => c.EnsureSuccessStatusCode = false)
                .ExecuteAsync();

            result.StatusCode.Should().Be(500);
        }

        private class TestClient : FluentHttpClient
        {
            public HttpRequestMessage RequestMessage { get; private set; }
            public HttpCompletionOption CompletionOption { get; private set; }

            public TestClient(string name, HttpClient client, FluentHttpClientOptions options)
                : base(name, client, options)
            {
            }

            public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, HttpCompletionOption completionOption, CancellationToken cancellationToken)
            {
                RequestMessage = requestMessage;
                CompletionOption = completionOption;
                return base.SendAsync(requestMessage, completionOption, cancellationToken);
            }
        }
    }
}
