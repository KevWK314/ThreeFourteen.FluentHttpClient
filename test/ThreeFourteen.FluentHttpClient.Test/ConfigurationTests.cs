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

namespace ThreeFourteen.FluentHttpClient.Test
{
    public class ConfigurationTests
    {
        private IFluentHttpClient _client;
        private ISerialization _serialization;
        private HttpResponseMessage _httpResponseMessage;

        public ConfigurationTests()
        {
            _serialization = Substitute.For<ISerialization>();
            _client = Substitute.For<IFluentHttpClient>();

            _httpResponseMessage = new HttpResponseMessage();
            _client.SendAsync(Arg.Any<HttpRequestMessage>(), Arg.Any<HttpCompletionOption>(), Arg.Any<CancellationToken>())
                .Returns(c => Task.FromResult(_httpResponseMessage));
        }

        [Fact]
        public async Task Config_WhenCompletionOptionsNotSet_ShouldDefault()
        {
            var response = await _client.Get("url").ExecuteAsync();

            response?.StatusCode.Should().Be(200);

            await _client.Received(1).SendAsync(Arg.Any<HttpRequestMessage>(), HttpCompletionOption.ResponseContentRead, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task ClientConfig_WhenCompletionOptionsSet_ShouldBeUsedWhenSending()
        {
            _client.Configuration.HttpCompletionOption = HttpCompletionOption.ResponseHeadersRead;
            var response = await _client.Get("url").ExecuteAsync();

            response?.StatusCode.Should().Be(200);

            await _client.Received(1).SendAsync(Arg.Any<HttpRequestMessage>(), HttpCompletionOption.ResponseHeadersRead, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task RequestConfig_WhenCompletionOptionsSet_ShouldBeUsedWhenSending()
        {
            var response = await _client.Get("url")
                .Configure(c => c.HttpCompletionOption = HttpCompletionOption.ResponseHeadersRead)
                .ExecuteAsync();

            response?.StatusCode.Should().Be(200);

            await _client.Received(1).SendAsync(Arg.Any<HttpRequestMessage>(), HttpCompletionOption.ResponseHeadersRead, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task ClientConfig_WhenSerialisationSet_ShouldBeUsedWhenSending()
        {
            _client.Configuration.Serialization = _serialization;
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
            _httpResponseMessage.StatusCode = System.Net.HttpStatusCode.InternalServerError;

            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => _client.Get("url").ExecuteAsync());
        }

        [Fact]
        public async Task ClientConfig_WhenEnsureSuccessSet_ShouldUseInResponse()
        {
            _httpResponseMessage.StatusCode = HttpStatusCode.InternalServerError;
            _client.Configuration.EnsureSuccessStatusCode = false;

            var result = await _client.Get("url").ExecuteAsync();

            result.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task RequestConfig_WhenEnsureSuccessSet_ShouldUseInResponse()
        {
            _httpResponseMessage.StatusCode = HttpStatusCode.InternalServerError;

            var result = await _client.Get("url")
                .Configure(c => c.EnsureSuccessStatusCode = false)
                .ExecuteAsync();

            result.StatusCode.Should().Be(500);
        }
    }
}
