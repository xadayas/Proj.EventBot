using EventBot.Entities.Models;
using EventBot.Entities.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace EventBot.Web.Controllers.Api
{

    [Authorize]
    public class NotificationsController : ApiController
    {
        private readonly EventServiceEf _eventService;

        public NotificationsController()
        {
            _eventService = new EventServiceEf();
        }

        public IEnumerable<Notification> GetNewNotifications()
        {
            var user = User.Identity.GetUserId();
            var notification = _eventService.GetNewNotificationsFor(user);

            return notification;
        }

        [HttpPost]
        public IHttpActionResult MarkAsRead()
        {
            var user = User.Identity.GetUserId();
            _eventService.MarkNotificationAsRead(user);

            return Ok();
        }
    }
}
