using System.Net.Http;

namespace FluentHttpClient
{
    public static class FluentHttpClientExtensions
    {
        public static RequestBuilder Get(this IFluentHttpClient client, string uri)
        {
            return new RequestBuilder(client, uri, HttpMethod.Get);
        }

        public static RequestBuilder Post(this IFluentHttpClient client, string uri)
        {
            return new RequestBuilder(client, uri, HttpMethod.Post);
        }

        public static RequestBuilder Put(this IFluentHttpClient client, string uri)
        {
            return new RequestBuilder(client, uri, HttpMethod.Put);
        }

        public static RequestBuilder Delete(this IFluentHttpClient client, string uri)
        {
            return new RequestBuilder(client, uri, HttpMethod.Delete);
        }

        public static RequestBuilder Head(this IFluentHttpClient client, string uri)
        {
            return new RequestBuilder(client, uri, HttpMethod.Head);
        }
    }
}
