using System.Net.Http;
using ThreeFourteen.FluentHttpClient.Serialize;

namespace ThreeFourteen.FluentHttpClient
{
    public class FluentHttpClientConfiguration
    {
        public virtual ISerialization Serialization { get; set; }
        public virtual bool? EnsureSuccessStatusCode { get; set; }
        public virtual HttpCompletionOption? HttpCompletionOption { get; set; }
    }
}
