using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using CGeers.Google.Resources;
using CGeers.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CGeers.Google
{
    public class Google : IUrlShortener
    {
        public delegate void GoogleCallback<T>(T data) where T : class, new();

        public void Shorten(string longUrl, UrlShortened callback)
        {
            var scheme = DetermineScheme(longUrl);
            if (scheme != null && longUrl.IndexOf(scheme, StringComparison.CurrentCultureIgnoreCase) == -1)
            {
                longUrl = string.Format("{0}://{1}", scheme, longUrl);
            }

            if (!IsValidUrl(longUrl))
            {
                callback(new Web.LongUrlResponse { StatusCode = 400, Error = Text.InvalidUrl });
                return;
            }

            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                callback(new Web.LongUrlResponse { StatusCode = 500, Error = Text.NoInternetConnection });
                return;
            }

            var requestUri = new StringBuilder(GoogleApi.EndPoint);
            requestUri.AppendFormat("?key={0}&", GoogleApi.ApiKey);

            var request = (HttpWebRequest)WebRequest.Create(requestUri.ToString());
            request.Method = "POST";
            request.ContentType = "application/json";

            request.BeginGetRequestStream(
                ar1 =>
                    {
                        using (var writer = new StreamWriter(request.EndGetRequestStream(ar1)))
                        {
                            writer.Write(JsonConvert.SerializeObject(new LongUrlRequest(longUrl)));
                        }

                        request.BeginGetResponse(
                            ar2 =>
                                {
                                    var googleResponse = new Web.LongUrlResponse { LongUrl = longUrl };
                                    try
                                    {
                                        var response = (HttpWebResponse) request.EndGetResponse(ar2);
                                        using (var reader = new StreamReader(response.GetResponseStream()))
                                        {
                                            var jsonResponse = reader.ReadToEnd();
                                            var shortUrl =
                                                JsonConvert.DeserializeObject<LongUrlResponse>(jsonResponse);
                                            googleResponse.StatusCode = 200;
                                            googleResponse.ShortUrl = shortUrl.Id;
                                        }
                                    }
                                    catch(WebException ex)
                                    {
                                        var response = ex.Response as HttpWebResponse;

                                        using (var reader = new StreamReader(response.GetResponseStream()))
                                        {
                                            var jsonResponse = reader.ReadToEnd();

                                            var jObject = JObject.Parse(jsonResponse);

                                            googleResponse.StatusCode = Int32.Parse(jObject["error"]["code"].ToString());

                                            googleResponse.Error = longUrl.StartsWith("http://goo.gl") ? 
                                                Text.AlreadyAShortened : 
                                                jObject["error"]["message"].ToString();
                                        }
                                    }

                                    callback(googleResponse);

                                }, null);

                    }, null);
        }

        private static bool IsValidUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            try
            {
                Uri test;
                return Uri.TryCreate(url, UriKind.Absolute, out test);
            }
            catch
            {
                return false;
            }
        }

        private static string DetermineScheme(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;

            try
            {
                var uri = new UriBuilder(url);
                return uri.Scheme;
            }
            catch (UriFormatException)
            {
                return null;
            }
        }
    }
}
