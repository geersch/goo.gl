using Newtonsoft.Json;

namespace CGeers.Google
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class GoogleError
    {
        [JsonProperty(PropertyName = "code")]
        public int StatusCode { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
