using EventBot.Entities.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventBot.Entities.Service.Models;

namespace EventBot.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly EventServiceEf _service;

        public HomeController()
        {
            _service = new EventServiceEf();
        }
        public ActionResult Index()
        {
            IpLocation location;
            var emptyLocation = new IpLocation()
            {
                city = "Vintergatan",
                country = "Världen"
            };
            try
            {
                location = IpLocator.GetIpLocation(Request.UserHostAddress);
                if (location == null || string.IsNullOrEmpty(location.city))
                {
                    location = emptyLocation;
                }
                    
            }
            catch (Exception)
            {
                location = emptyLocation;
            }
            var events = _service.SearchEvents("", location: new LocationModel { Latitude = location.lat, Longitude = location.lon }, sortBy: EventSortBy.Distance, modulus: 4);
            ViewData["HotEvents"] = events.OrderBy(o => o.VisitCount).ToArray();
            return View(location);
        }

        public ActionResult GetUpComingEvents()
        {
            var events = _service.GetAllUpComingEvents();
            return View(events);
        }
    }
}