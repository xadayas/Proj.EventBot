using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EventBot.Entities.Service.Models;
using EventBot.Web.Models;

namespace EventBot.Web.Controllers
{
    public class MapController : Controller
    {
        // GET: Map
        public PartialViewResult Index(LocationViewModel location)
        {
            
            return PartialView(location);
        }

        public PartialViewResult Static(LocationViewModel location)
        {
            return PartialView(location);
        }
    }
}