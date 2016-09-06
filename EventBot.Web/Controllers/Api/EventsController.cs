﻿using EventBot.Entities.Service;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EventBot.Web.Controllers.Api
{
    [Authorize]
    public class EventsController : ApiController
    {
        private readonly EventServiceEf _service;

        public EventsController()
        {
            _service = new EventServiceEf();
        }

        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var userId = User.Identity.GetUserId();

            var eventModel = _service.GetEvent(id);
            if (eventModel != null && eventModel.UserId == userId)
            {
                eventModel.IsCanceled = true;
                _service.CreateOrUpdateEvent(eventModel);
            }
            return Ok();
        }
    }
}