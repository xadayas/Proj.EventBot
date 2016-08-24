﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
    }
}