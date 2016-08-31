using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public ActionResult GetJoinedEvents()
        {
            var userId = User.Identity.GetUserId();

            return View(_service.GetAllUpcomingEventsFor(userId));
        }

        public ActionResult Search(string query, int persons = 0, int cost = Int32.MaxValue,int sortBy=0)
        {
            var sortByParsed = (EventSortBy) sortBy;
            
            
            var events = _service.SearchEvents(query:query,maxCost: cost, minPlaces:persons,sortBy:sortByParsed);
            return PartialView(events);
        }
        // GET: Event/Details/5
        public ActionResult Details(int id)
        {
            var ev = _service.GetEvent(id);
            _service.AddVisitorToEvent(id);
            return View(ev);
        }
        // GET: Event/Details/5
        public ActionResult Details2(int id)
        {
            var ev = _service.GetEvent(id);
            _service.AddVisitorToEvent(id);
            return View(ev);
        }

        // GET: Event/Create
        public ActionResult Create()
        {
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
            if (model.Tags == null) model.Tags = string.Empty;
            var eventTypes = _service.GetEventTypes();
            if (string.IsNullOrWhiteSpace(model.Location.Name))
                model.Location.Name = string.Empty;
            else
                model.Location = GeoCode.GoogleGeoCode(model.Location.Name).FirstOrDefault() ?? new LocationViewModel { Name = model.Location.Name };

            model.UserId = User.Identity.GetUserId();

            //TODO select starttime and endtime
            _service.CreateOrUpdateEvent(new EventModel
            {
                UserId = model.UserId,
                Title = model.Title,
                Description = model.Description,
                ParticipationCost = model.ParticipationCost,
                MaxAttendees = model.MaxAttendees,
                Location = new LocationModel
                {
                    Name = model.Location.Name,
                    Latitude = model.Location.Latitude,
                    Longitude = model.Location.Longitude
                },
                EventTypes = model.Tags.Split(',').Select(s => new EventTypeModel
                {
                    Id = eventTypes.FirstOrDefault(f => f.Name == s)?.Id ?? 0,
                    Name = s
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
            // check if event is saved in session variable
            EventViewModel model = Session["imageUploadEventSave"] as EventViewModel;
            var editEvent = _service.GetEvent(id);

            // else get from db.
            if (model == null)
            {

                model = new EventViewModel
                {
                    Id = editEvent.Id,
                    Title = editEvent.Title,
                    Description = editEvent.Description,
                    StartDate = editEvent.StartDate,
                    EndDate = editEvent.EndDate,
                    ImageId = editEvent.ImageId,
                    UserId = editEvent.UserId,
                    IsCanceled = editEvent.IsCanceled,
                    ParticipationCost = editEvent.ParticipationCost,
                    MaxAttendees = editEvent.MaxAttendees,
                    Tags = editEvent.EventTypes.Select(s => s.Name).Pipe(p => String.Join(",", p)), //Aggregate((a, b) => a + ',' + b),
                    Location = new LocationViewModel
                    {
                        Id = editEvent.Location.Id,
                        Name = editEvent.Location.Name,
                        Latitude = editEvent.Location.Latitude,
                        Longitude = editEvent.Location.Longitude,
                        Altitude = editEvent.Location.Altitude
                    }
                };
            }
            if (model.UserId == null) model.UserId = editEvent.UserId;
            if (model.UserId != User.Identity.GetUserId()) return new HttpStatusCodeResult(HttpStatusCode.Unauthorized, "Access denied");
            return View(model);
        }

        // POST: Event/Edit/5
        [HttpPost]
        public ActionResult Edit(EventViewModel model)
        {
            // Check that user owns the event
            var originalEvent = _service.GetEvent(model.Id);
            if (originalEvent == null || originalEvent.UserId != User.Identity.GetUserId())
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized, "Access denied");
            if (!ModelState.IsValid)
                return View(model);

            model.UserId = User.Identity.GetUserId();
            // get latitude and longitude
            if (string.IsNullOrWhiteSpace(model.Location.Name))
                model.Location.Name = string.Empty;
            else
                model.Location = GeoCode.GoogleGeoCode(model.Location.Name).FirstOrDefault() ?? new LocationViewModel { Name = model.Location.Name };

            if (model.Tags == null) model.Tags = string.Empty;

            var eventTypes = _service.GetEventTypes();
            //translate to dto
            var editedEvent = new EventModel
            {
                UserId = model.UserId,
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                ImageId = model.ImageId,
                IsCanceled = model.IsCanceled,
                MeetingPlace = model.Location.Name,
                ParticipationCost = model.ParticipationCost,
                MaxAttendees = model.MaxAttendees,
                Location = new LocationModel
                {
                    Id = model.Location.Id,
                    Latitude = model.Location.Latitude,
                    Longitude = model.Location.Longitude,
                    Altitude = model.Location.Altitude,
                    Name = model.Location.Name
                },
                EventTypes = model.Tags.Split(',').Select(s => new EventTypeModel
                {
                    Id = eventTypes.FirstOrDefault(f => f.Name == s)?.Id ?? 0,
                    Name = s
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
