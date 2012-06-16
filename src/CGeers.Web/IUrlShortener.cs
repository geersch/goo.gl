namespace CGeers.Web
{
    public interface IUrlShortener
    {
        void Shorten(string longUrl, UrlShortened callback);
    }
}
