using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EventBot.Entities.Service;
using EventBot.Web.Models;

namespace EventBot.Web.Controllers.Api
{
    public class TagsController : ApiController
    {
        private readonly EventServiceEf _service;

        public TagsController()
        {
            _service = new EventServiceEf();
        }
        public IEnumerable<string> GetAllTags()
        {
            return _service.GetEventTypes().Select(s=> s.Name);
        }
    }
}
