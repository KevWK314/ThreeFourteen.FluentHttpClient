using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using ThreeFourteen.FluentHttpClient.Test.Tools;
using Xunit;

namespace ThreeFourteen.FluentHttpClient.Test
{
    public class RequestMethodTests
    {
        [Fact]
        public async Task Get()
        {
            var httpClientTester = new HttpClientTester()
                .SetResponseStatusCode(System.Net.HttpStatusCode.OK);

            await new FluentHttpClient("Test", httpClientTester.Client)
               .Get("url")
               .ExecuteAsync();

            httpClientTester.RequestMessage.Method.Should().Be(HttpMethod.Get);
        }

        [Fact]
        public async Task Post()
        {
            var httpClientTester = new HttpClientTester()
                .SetResponseStatusCode(System.Net.HttpStatusCode.OK);

            await new FluentHttpClient("Test", httpClientTester.Client)
               .Post("url")
               .ExecuteAsync();

            httpClientTester.RequestMessage.Method.Should().Be(HttpMethod.Post);
        }

        [Fact]
        public async Task Put()
        {
            var httpClientTester = new HttpClientTester()
                .SetResponseStatusCode(System.Net.HttpStatusCode.OK);

            await new FluentHttpClient("Test", httpClientTester.Client)
               .Put("url")
               .ExecuteAsync();

            httpClientTester.RequestMessage.Method.Should().Be(HttpMethod.Put);
        }

        [Fact]
        public async Task Delete()
        {
            var httpClientTester = new HttpClientTester()
                .SetResponseStatusCode(System.Net.HttpStatusCode.OK);

            await new FluentHttpClient("Test", httpClientTester.Client)
               .Delete("url")
               .ExecuteAsync();

            httpClientTester.RequestMessage.Method.Should().Be(HttpMethod.Delete);
        }

        [Fact]
        public async Task Head()
        {
            var httpClientTester = new HttpClientTester()
                .SetResponseStatusCode(System.Net.HttpStatusCode.OK);

            await new FluentHttpClient("Test", httpClientTester.Client)
               .Head("url")
               .ExecuteAsync();

            httpClientTester.RequestMessage.Method.Should().Be(HttpMethod.Head);
        }
    }
}
