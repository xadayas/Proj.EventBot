using System.Net;
using System.Web.Helpers;

namespace EventBot.Web
{
    public class IpLocator
    {
        private const string _mainHost = "http://ip-api.com/json/";
        public static void GetLocation(string host, out double lat, out double lon)
        {
            var jsonString = new WebClient().DownloadString(_mainHost + host);
            var p = Json.Decode<IpLocation>(jsonString);
            lat = p.lat;
            lon = p.lon;
        }
        public static IpLocation GetIpLocation(string host)
        {
            var jsonString = new WebClient().DownloadString(_mainHost + host);
            return Json.Decode<IpLocation>(jsonString);
        }
    }
    public class IpLocation
    {
        public string _as { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string countryCode { get; set; }
        public string isp { get; set; }
        public float lat { get; set; }
        public float lon { get; set; }
        public string org { get; set; }
        public string query { get; set; }
        public string region { get; set; }
        public string regionName { get; set; }
        public string status { get; set; }
        public string timezone { get; set; }
        public string zip { get; set; }
    }
}