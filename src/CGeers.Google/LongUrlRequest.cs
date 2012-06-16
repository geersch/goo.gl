using System;
using Newtonsoft.Json;

namespace CGeers.Google
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LongUrlRequest
    {
        public LongUrlRequest(string url)
        {
            Url = url;
        }

        public LongUrlRequest(Uri uri)
            : this(uri.ToString())
        {
        }

        [JsonProperty(PropertyName = "longUrl")]
        public string Url { get; set; }
    }
}
