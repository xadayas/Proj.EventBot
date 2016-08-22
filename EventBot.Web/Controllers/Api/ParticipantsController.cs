using EventBot.Entities.Models;
using EventBot.Entities.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using EventBot.Entities;

namespace EventBot.Web.Controllers.Api
{

    [Authorize]
    public class ParticipantsController : ApiController
    {
        private readonly EventServiceEf _eventService;

        public ParticipantsController()
        {
            _eventService = new EventServiceEf();
        }

        //Verifiera in parameter
        [HttpPost]
        public IHttpActionResult Attend(int eventId)
        {
            var userId = User.Identity.GetUserId();
            if (_eventService.CheckParticipant(userId, eventId))
                return BadRequest("Participant already exists.");

            _eventService.JoinEvent(userId, eventId);

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeleteAttendee(int eventId)
        {
            var userId = User.Identity.GetUserId();
            if (!_eventService.CheckParticipant(userId, eventId))
                return NotFound();

            _eventService.LeaveEvent(userId, eventId);
            return Ok();
        }
    }
}
