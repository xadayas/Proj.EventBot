using System;
using EventBot.Entities.Service.Interfaces;
using EventBot.Entities.Service.Models;
using NUnit.Framework;

namespace EventBot.Entities.Service.Test
{
    [TestFixture]
    public class ServiceTests
    {
        private readonly IEventService _service = new EventServiceEf();

        [Test]
        public void CanAddEvent()
        {
            // TODO get user id?
            // TODO add asserts
            var testEvent = new EventModel
            {
                Title = "TestTitle",
                Description = "Test Event Description",
                StartDate = DateTime.Now + TimeSpan.FromDays(7),
                EndDate = DateTime.Now + TimeSpan.FromDays(7) + TimeSpan.FromHours(3),
                MeatingPlace = "Skogen brevid Ängen"
            };
            Assert.DoesNotThrow(() =>
            {
                _service.CreateOrUpdateEvent(testEvent);
            });
            Assert.That(testEvent.Id!=0);
            string msg = testEvent.Id != 0 ? $"Event created successful, assigned id = {testEvent.Id}" : "Failed creating Event";
            Console.WriteLine(msg);
        }
    }
}