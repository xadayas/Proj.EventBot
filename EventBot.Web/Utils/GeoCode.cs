using System;
using System.Collections.Generic;
using System.Globalization;
using EventBot.Entities.Service.Models;
using EventBot.Web.Models;

namespace EventBot.Web.Utils
{
    public class GeoCode
    {
        public static IEnumerable<LocationViewModel> GoogleGeoCode(string address)
        {
            string url = "http://maps.googleapis.com/maps/api/geocode/json?sensor=true&address=";

            dynamic googleResults = new Uri(url + address).GetDynamicJsonObject();
            foreach (var result in googleResults.results)
            {
                yield return new LocationViewModel
                {
                    Name = result.formatted_address,
                    Latitude = result.geometry.location.lat,
                    Longitude = result.geometry.location.lng
                };
            }
        }
        public static LocationModel IpToLocation(string ipaddr)
        {
            var ipLocation = IpLocator.GetIpLocation(ipaddr);
            return new LocationModel
            {
                Latitude = ipLocation.lat,
                Longitude = ipLocation.lon,
                Name = ipLocation.city
            };
        }
    }
}