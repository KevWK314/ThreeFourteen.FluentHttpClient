using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ThreeFourteen.FluentHttpClient.Serialize
{
    public class JsonHttpContent<T> : HttpContent
    {
        private readonly T _request;
        private readonly JsonSerializer _serializer;

        public JsonHttpContent(T request, JsonSerializer serializer)
        {
            _request = request;
            _serializer = serializer;
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            using (var writer = new StreamWriter(stream, Encoding.Default, 1024, true))
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                _serializer.Serialize(jsonWriter, _request);

                jsonWriter.Flush();
            }

            return Task.CompletedTask;
        }

        protected override bool TryComputeLength(out long length)
        {
            length = -1;
            return false;
        }
    }
}
