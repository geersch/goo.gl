using Newtonsoft.Json;

namespace CGeers.Google
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LongUrlResponse
    {
        [JsonProperty(PropertyName = "kind")]
        public string Kind { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "longUrl")]
        public string LongUrl { get; set; }
    }
}
