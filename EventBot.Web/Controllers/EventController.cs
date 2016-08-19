using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventBot.Entities.Service;
using EventBot.Entities.Service.Interfaces;
using EventBot.Entities.Service.Models;
using Microsoft.AspNet.Identity;

namespace EventBot.Web.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly IEventService _service = new EventServiceEf();
        // GET: Event
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserEvents()
        {
            return View(_service.GetUserCreatedEvents(User.Identity.GetUserId()));
        }
        public ActionResult UserSubscribedEvents()
        {
            return View();
        }
        
        public ActionResult Search(string query)
        {
            return PartialView(_service.SearchEvents(query));
        }
        // GET: Event/Details/5
        public ActionResult Details(int id)
        {
            var ev = _service.GetEvent(id);
            return View(ev);
        }

        // GET: Event/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Event/Create
        [HttpPost]
        public ActionResult Create(EventModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.UserId = User.Identity.GetUserId();
            _service.CreateOrUpdateEvent(model);
            return RedirectToAction("UserEvents");
        }

        // GET: Event/Edit/5
        public ActionResult Edit(int id)
        {
            return View(_service.GetEvent(id));
        }

        // POST: Event/Edit/5
        [HttpPost]
        public ActionResult Edit(EventModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            model.UserId = User.Identity.GetUserId();
            _service.CreateOrUpdateEvent(model);
            return RedirectToAction("UserEvents");
        }

        // GET: Event/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Event/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
