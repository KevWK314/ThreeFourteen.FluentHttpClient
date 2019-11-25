using Newtonsoft.Json;
using ThreeFourteen.FluentHttpClient.Serialize;

namespace ThreeFourteen.FluentHttpClient
{
    public static class RequestBuilderJsonSerializationExtensions
    {
        public static RequestBuilder WithJsonSerializer(this RequestBuilder requestBuilder)
        {
            requestBuilder.UpdateConfiguration(c => c.Serialization = new JsonSerialization());

            return requestBuilder;
        }

        public static RequestBuilder WithJsonSerializer(this RequestBuilder requestBuilder, JsonSerializerSettings settings)
        {
            requestBuilder.UpdateConfiguration(c => c.Serialization = new JsonSerialization(settings));

            return requestBuilder;
        }

        public static RequestBuilder WithJsonStreamSerializer(this RequestBuilder requestBuilder)
        {
            requestBuilder.UpdateConfiguration(c => c.Serialization = new JsonStreamSerialization());

            return requestBuilder;
        }

        public static RequestBuilder WithJsonStreamSerializer(this RequestBuilder requestBuilder, JsonSerializerSettings settings)
        {
            requestBuilder.UpdateConfiguration(c => c.Serialization = new JsonStreamSerialization(settings));

            return requestBuilder;
        }
    }
}
