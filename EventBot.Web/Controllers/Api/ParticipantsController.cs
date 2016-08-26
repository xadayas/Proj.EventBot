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
            _eventService.JoinEvent(userId, id);
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
            var attendStatus = new { Attending = _eventService.CheckParticipant(User.Identity.GetUserId(), id) };
            return Json(attendStatus, JsonRequestBehavior.AllowGet);
        }
    }
}
