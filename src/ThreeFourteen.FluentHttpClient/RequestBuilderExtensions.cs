using System;
using System.Net.Http;
using FluentHttpClient.Serialize;

namespace FluentHttpClient
{
    public static class RequestBuilderExtensions
    {
        public static RequestBuilder WithHeader(this RequestBuilder requestBuilder, string name, params string[] values)
        {
            requestBuilder.OnRequest(r => r.Headers.Add(name, values));

            return requestBuilder;
        }

        public static RequestBuilder WithSerializer(this RequestBuilder requestBuilder, ISerialization serialization)
        {
            requestBuilder.UpdateConfiguration(c => c.Serialization = serialization);

            return requestBuilder;
        }
    }
}