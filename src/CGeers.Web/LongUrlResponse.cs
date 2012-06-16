namespace CGeers.Web
{
    public class LongUrlResponse
    {
        public int StatusCode { get; set; }

        public string LongUrl { get; set; }

        public string ShortUrl { get; set; }

        public string Error { get; set; }
    }
}