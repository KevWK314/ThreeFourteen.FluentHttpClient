using System.Net.Http;
using System.Threading;
using FluentHttpClient.Serialize;

namespace FluentHttpClient
{
    public class FluentHttpClientConfiguration
    {
        public virtual ISerialization Serialization { get; set; }
        public virtual bool? EnsureSuccessStatusCode { get; set; }
        public virtual HttpCompletionOption? HttpCompletionOption { get; set; }
        public virtual CancellationToken? CancellationToken { get; set; }
    }
}
