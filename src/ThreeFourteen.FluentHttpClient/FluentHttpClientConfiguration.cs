using System.Net.Http;
using System.Threading;
using ThreeFourteen.FluentHttpClient.Serialize;

namespace ThreeFourteen.FluentHttpClient
{
    public class FluentHttpClientConfiguration
    {
        public virtual ISerialization Serialization { get; set; }
        public virtual bool? EnsureSuccessStatusCode { get; set; }
        public virtual HttpCompletionOption? HttpCompletionOption { get; set; }
        public virtual CancellationToken? CancellationToken { get; set; }
    }
}
