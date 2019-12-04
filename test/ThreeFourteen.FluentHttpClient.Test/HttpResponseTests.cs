using System.Threading.Tasks;
using FluentAssertions;
using ThreeFourteen.FluentHttpClient.Test.Model;
using ThreeFourteen.FluentHttpClient.Test.Tools;
using Xunit;

namespace ThreeFourteen.FluentHttpClient.Test
{
    public class HttpResponseTests
    {
        [Fact]
        public async Task Execute_WhenSuccessResult_ShouldReturnErrorResponse()
        {
            var httpClientTester = new HttpClientTester()
                .SetResponseStatusCode(System.Net.HttpStatusCode.OK)
                .SetResponseContent(new Person("Sandra"));

            var response = await new FluentHttpClient("Test", httpClientTester.Client)
                .Get("url")
                .ExecuteAsync<Person>();

            response?.StatusCode.Should().Be(200);
            response?.Result?.Name.Should().Be("Sandra");
            response?.IsSuccessStatusCode.Should().BeTrue();
            response?.ReasonPhrase.Should().Be("OK");
        }

        [Fact]
        public async Task Execute_WhenErrorResult_ShouldReturnErrorResponse()
        {
            var httpClientTester = new HttpClientTester()
                .SetResponseStatusCode(System.Net.HttpStatusCode.InternalServerError);

            var response = await new FluentHttpClient("Test", httpClientTester.Client)
                .Get("url")
                .Configure(x => x.EnsureSuccessStatusCode = false)
                .ExecuteAsync<Person>();

            response?.StatusCode.Should().Be(500);
            response?.Result.Should().BeNull();
            response?.IsSuccessStatusCode.Should().BeFalse();
            response?.ReasonPhrase.Should().Be("Internal Server Error");
        }

        [Fact]
        public async Task Execute_WhenSuccessNoResult_ShouldReturnErrorResponse()
        {
            var httpClientTester = new HttpClientTester()
                .SetResponseStatusCode(System.Net.HttpStatusCode.Created);

            var response = await new FluentHttpClient("Test", httpClientTester.Client)
                .Put("url")
                .ExecuteAsync<Person>();

            response?.StatusCode.Should().Be(201);
            response?.Result?.Name.Should().Be("Sandra");
            response?.IsSuccessStatusCode.Should().BeTrue();
            response?.ReasonPhrase.Should().Be("Created");
        }

        [Fact]
        public async Task Execute_WhenErrorNoResult_ShouldReturnErrorResponse()
        {
            var httpClientTester = new HttpClientTester()
                .SetResponseStatusCode(System.Net.HttpStatusCode.Unauthorized);

            var response = await new FluentHttpClient("Test", httpClientTester.Client)
                .Put("url")
                .Configure(x => x.EnsureSuccessStatusCode = false)
                .ExecuteAsync<Person>();

            response?.StatusCode.Should().Be(401);
            response?.Result.Should().BeNull();
            response?.IsSuccessStatusCode.Should().BeFalse();
            response?.ReasonPhrase.Should().Be("Unauthorized");
        }
    }
}
