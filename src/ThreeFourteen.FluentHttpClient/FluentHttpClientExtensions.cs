using System.Net.Http;

namespace ThreeFourteen.FluentHttpClient
{
    public static class FluentHttpClientExtensions
    {
        public static RequestBuilder Get(this FluentHttpClient client, string uri)
        {
            return new RequestBuilder(client, uri, HttpMethod.Get);
        }

        public static RequestBuilder Post(this FluentHttpClient client, string uri)
        {
            return new RequestBuilder(client, uri, HttpMethod.Post);
        }

        public static RequestBuilder Put(this FluentHttpClient client, string uri)
        {
            return new RequestBuilder(client, uri, HttpMethod.Put);
        }

        public static RequestBuilder Delete(this FluentHttpClient client, string uri)
        {
            return new RequestBuilder(client, uri, HttpMethod.Delete);
        }

        public static RequestBuilder Head(this FluentHttpClient client, string uri)
        {
            return new RequestBuilder(client, uri, HttpMethod.Head);
        }
    }
}
