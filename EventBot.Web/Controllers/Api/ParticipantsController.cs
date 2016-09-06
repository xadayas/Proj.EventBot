using System;
using System.Net;
using System.Web.Mvc;
using EventBot.Entities.Service;
using Microsoft.AspNet.Identity;

namespace EventBot.Web.Controllers.Api
{

    [System.Web.Http.Authorize]
    public class ParticipantsController : Controller
    {
        private readonly EventServiceEf _eventService;

        public ParticipantsController()
        {
            _eventService = new EventServiceEf();
        }
        [HttpPost]
        public ActionResult Attend(int id)
        {
            var userId = User.Identity.GetUserId();
            try
            {
                _eventService.JoinEvent(userId, id);
            }
            catch (InvalidOperationException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
            
            return new HttpStatusCodeResult(200);
        }
        [HttpPost]
        public ActionResult UnAttend(int id)
        {
            var userId = User.Identity.GetUserId();
            if (!_eventService.CheckParticipant(userId, id))
                return new HttpStatusCodeResult(404);
            _eventService.LeaveEvent(userId,id);
            return new HttpStatusCodeResult(200);
        }
        [HttpGet]
        public JsonResult IsAttending(int id)
        {
            var tempEvent = _eventService.GetEvent(id);
            var availableSlots = tempEvent.MaxAttendees - tempEvent.UserCount;
            if (tempEvent.MaxAttendees == 0)
                availableSlots = int.MaxValue;
            var attendStatus = new { Attending = _eventService.CheckParticipant(User.Identity.GetUserId(), id), AvailableSlots = availableSlots < 0 ? int.MaxValue : availableSlots };
            return Json(attendStatus, JsonRequestBehavior.AllowGet);
        }
    }
}
