using System.Collections.Generic;

namespace ThreeFourteen.FluentHttpClient
{
    public class HttpResponse
    {
        public int StatusCode { get; internal set; }

        public KeyValuePair<string, string[]>[] ResponseHeaders { get; internal set; }

        public bool IsSuccessStatusCode { get; internal set; }

        public string ReasonPhrase { get; internal set; }
    }

    public class HttpResponse<T> : HttpResponse
    {
        public T Result { get; internal set; }
    }
}
