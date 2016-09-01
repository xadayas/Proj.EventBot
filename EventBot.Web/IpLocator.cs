using System.Net;
using System.Web.Helpers;

namespace EventBot.Web
{
    public class IpLocator
    {
        public static void GetLocation(string host, out double lat, out double lon)
        {
            var jsonString = new WebClient().DownloadString("http://www.freegeoip.net/json/" + host);
            var p =  Json.Decode<IpLocation>(jsonString);
            lat = p.latitude;
            lon = p.longitude;
        }

        public static IpLocation GetIpLocation(string host)
        {
            var jsonString = new WebClient().DownloadString("http://www.freegeoip.net/json/" + host);
            return Json.Decode<IpLocation>(jsonString);
        }
    }
    public class IpLocation
    {
        public string ip { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public string region_code { get; set; }
        public string region_name { get; set; }
        public string city { get; set; }
        public string zip_code { get; set; }
        public string time_zone { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public int metro_code { get; set; }
    }
}