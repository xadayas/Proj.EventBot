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
            ViewData["HotEvents"] = _service.SearchEvents("");
            var location = IpLocator.GetIpLocation(Request.UserHostAddress);
            return View(location);
        }

        public ActionResult GetUpComingEvents()
        {
            var events = _service.GetAllUpComingEvents();
            return View(events);
        }
    }
}