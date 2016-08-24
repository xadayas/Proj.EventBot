using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventBot.Web.Models
{
    public class LocationViewModel
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
    }
}