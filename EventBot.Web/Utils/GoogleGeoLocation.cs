using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Helpers;

namespace EventBot.Web.Utils
{
    public static class GoogleGeoLocation
    {
        private static string _mainHost = "http://maps.googleapis.com/maps/api/geocode/json?sensor=true&address=";

        public static GoogleLocation AddressToLocation(this string address)
        {
            var jsonString = new WebClient().DownloadString(_mainHost + address);
            var bytes = Encoding.Default.GetBytes(jsonString);
            jsonString = Encoding.UTF8.GetString(bytes);
            return Json.Decode<GoogleLocation>(jsonString);
        }
    }

    public class GoogleLocation
    {
        public Result[] results { get; set; }
        public string status { get; set; }
    }

    public class Result
    {
        public Address_Components[] address_components { get; set; }
        public string formatted_address { get; set; }
        public Geometry geometry { get; set; }
        public string place_id { get; set; }
        public string[] types { get; set; }
    }

    public class Geometry
    {
        public Location location { get; set; }
        public string location_type { get; set; }
        public Viewport viewport { get; set; }
    }

    public class Location
    {
        public float lat { get; set; }
        public float lng { get; set; }
    }

    public class Viewport
    {
        public Northeast northeast { get; set; }
        public Southwest southwest { get; set; }
    }

    public class Northeast
    {
        public float lat { get; set; }
        public float lng { get; set; }
    }

    public class Southwest
    {
        public float lat { get; set; }
        public float lng { get; set; }
    }

    public class Address_Components
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public string[] types { get; set; }
    }


}