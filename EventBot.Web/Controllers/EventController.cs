using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EventBot.Entities.Service;
using EventBot.Entities.Service.Interfaces;
using EventBot.Entities.Service.Models;
using EventBot.Web.Models;
using EventBot.Web.Utils;
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
        // GET: Event/Details/5
        public ActionResult Details2(int id)
        {
            var ev = _service.GetEvent(id);
            return View(ev);
        }

        // GET: Event/Create
        public ActionResult Create()
        {
            ViewBag.EventTypes = _service.GetEventTypes().Select(s => new EventTypeViewModel
            {
                Id = s.Id,
                Name = s.Name
            });
            EventViewModel model = Session["imageUploadEventSave"] as EventViewModel;
            Session["imageUploadEventSave"] = null;
            if (model == null) model = new EventViewModel();
            return View(model);
        }

        // POST: Event/Create
        [HttpPost]
        public ActionResult Create(EventViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var eventTypes = _service.GetEventTypes();
            model.Location = GeoCode.GoogleGeoCode(model.Location.Name).FirstOrDefault() ?? new LocationViewModel { Name = model.Location.Name };

            model.UserId = User.Identity.GetUserId();

            //TODO select starttime and endtime
            _service.CreateOrUpdateEvent(new EventModel
            {
                UserId = model.UserId,
                Title = model.Title,
                Description = model.Description,
                Location = new LocationModel
                {
                    Name = model.Location.Name,
                    Latitude = model.Location.Latitude,
                    Longitude = model.Location.Longitude
                },
                EventTypes = model.EventTypes.Select(s => new EventTypeModel
                {
                    Id = s,
                    Name = eventTypes.FirstOrDefault(f => f.Id == s)?.Name ?? ""
                }).ToArray(),
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                ImageId = model.ImageId,
                MeetingPlace = model.Location.Name
            });
            return RedirectToAction("UserEvents");
        }

        // GET: Event/Edit/5
        public ActionResult Edit(int id)
        {
            var editEvent = _service.GetEvent(id);

            // check if returning from image upload
            if (Session["EditImageId"] is int)
            {
                editEvent.ImageId = (int)Session["EditImageId"];
                Session["EditImageId"] = null;
            }


            if (editEvent.UserId == User.Identity.GetUserId())
            {
                ViewBag.EventTypes = _service.GetEventTypes().Select(s => new EventTypeViewModel
                {
                    Id = s.Id,
                    Name = s.Name
                });
                var editEventViewModel = new EventViewModel
                {
                    Id = editEvent.Id,
                    Title = editEvent.Title,
                    Description = editEvent.Description,
                    StartDate = editEvent.StartDate,
                    EndDate = editEvent.EndDate,
                    ImageId = editEvent.ImageId,
                    UserId = editEvent.UserId,
                    IsCanceled = editEvent.IsCanceled,
                    EventTypes = editEvent.EventTypes.Select(s=>s.Id).ToArray(),
                    Location = new LocationViewModel
                    {
                        Id = editEvent.Location.Id,
                        Name = editEvent.Location.Name,
                        Latitude = editEvent.Location.Latitude,
                        Longitude = editEvent.Location.Longitude,
                        Altitude = editEvent.Location.Altitude
                    }
                };
                return View(editEventViewModel);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized, "Access denied");
        }

        // POST: Event/Edit/5
        [HttpPost]
        public ActionResult Edit(EventViewModel model)
        {
            // Check that user owns the event
            var originalEvent = _service.GetEvent(model.Id);
            if (originalEvent==null||originalEvent.UserId!=User.Identity.GetUserId())
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized, "Access denied");
            if (!ModelState.IsValid)
                return View(model);
            model.UserId = User.Identity.GetUserId();
            // get latitude and longitude
            model.Location = GeoCode.GoogleGeoCode(model.Location.Name).FirstOrDefault() ?? new LocationViewModel { Name = model.Location.Name };

            var eventTypes = _service.GetEventTypes();
            //translate to dto
            var editedEvent = new EventModel
            {
                UserId = model.UserId,
                Id=model.Id,
                Title = model.Title,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                ImageId = model.ImageId,
                IsCanceled = model.IsCanceled,
                MeetingPlace = model.Location.Name,
                Location = new LocationModel
                {
                    Id = model.Location.Id,
                    Latitude = model.Location.Latitude,
                    Longitude = model.Location.Longitude,
                    Altitude = model.Location.Altitude,
                    Name = model.Location.Name
                },
                EventTypes = model.EventTypes.Select(s => new EventTypeModel
                {
                    Id = s,
                    Name = eventTypes.FirstOrDefault(f => f.Id == s)?.Name ?? ""
                }).ToArray()
            };
            _service.CreateOrUpdateEvent(editedEvent);
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
