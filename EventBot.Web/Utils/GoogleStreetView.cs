using System.Globalization;
using System.Net;
using EventBot.Web.Models;

namespace EventBot.Web.Utils
{
    public static class GoogleStreetView
    {
        public static byte[] GetStreetView(LocationViewModel location)
        {
            var bytes =
                new WebClient().DownloadData(
                    $"http://maps.googleapis.com/maps/api/streetview?size=640x640&location={location.Latitude.ToString(CultureInfo.InvariantCulture)},{location.Longitude.ToString(CultureInfo.InvariantCulture)}&fov=120&heading=235&pitch=10&sensor=false");
            return bytes;
        }
        public static byte[] GetMapImage(LocationViewModel location)
        {
            var bytes =
                new WebClient().DownloadData(
                    $"https://maps.googleapis.com/maps/api/staticmap?center={location.Latitude.ToString(CultureInfo.InvariantCulture)},{location.Longitude.ToString(CultureInfo.InvariantCulture)}&zoom=15&size=640x640&maptype=roadmap&markers=color:red%7Clabel:S|{location.Latitude.ToString(CultureInfo.InvariantCulture)},{location.Longitude.ToString(CultureInfo.InvariantCulture)}&key=AIzaSyALGUncOuBetS0vXnVDMrnJC-JSZA65kpU");
            return bytes;
        }
    }
}