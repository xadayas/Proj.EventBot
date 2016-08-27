using EventBot.Entities.Models;
using EventBot.Entities.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EventBot.Entities.Service.Models;
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

        public IEnumerable<NotificationModel> GetNewNotifications()
        {
            var user = User.Identity.GetUserId();
            var notification = _eventService.GetNewNotificationsFor(user);

            return notification;
        }

        //[HttpPost]
        //public IHttpActionResult MarkAllAsRead()
        //{
        //    var user = User.Identity.GetUserId();
        //    _eventService.MarkAllNotificationsAsRead(user);

        //    return Ok();
        //}
        [HttpPost]
        public IHttpActionResult MarkSingleAsRead(int id)
        {
            //int notId;
            //Int32.TryParse(id, out notId);
            if(id==0)return NotFound();
            var user = User.Identity.GetUserId();
            _eventService.MarkNotificationAsRead(id,user);

            return Ok();
        }
    }
}
