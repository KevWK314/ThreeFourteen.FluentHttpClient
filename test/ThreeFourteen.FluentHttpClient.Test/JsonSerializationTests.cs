using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ThreeFourteen.FluentHttpClient.Serialize;
using ThreeFourteen.FluentHttpClient.Test.Model;
using Xunit;

namespace ThreeFourteen.FluentHttpClient.Test
{
    public class JsonSerializationTests
    {
        private JsonSerialization _sut;

        public JsonSerializationTests()
        {
            _sut = new JsonSerialization(
                new JsonSerializerSettings
                {
                    Formatting = Formatting.None,
                    ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() }
                });
        }

        [Fact]
        public async Task Serialize()
        {
            var content = await _sut.Serialize(new Person("Happy"));
            var result = await content.ReadAsStringAsync();

            result.Should().Be(@"{""name"":""Happy""}");
        }

        [Fact]
        public async Task Deserialize()
        {
            var content = new StringContent(@"{""name"":""Happy""}");
            var result = await _sut.Deserialize<Person>(content);

            result?.Name.Should().Be("Happy");
        }
    }
}
