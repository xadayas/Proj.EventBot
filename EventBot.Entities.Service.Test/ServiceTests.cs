using System;
using System.Linq;
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
                MeetingPlace = "Skogen brevid Ängen"
            };
            Assert.DoesNotThrow(() =>
            {
                _service.CreateOrUpdateEvent(testEvent);
            });
            Assert.That(testEvent.Id!=0);
            string msg = testEvent.Id != 0 ? $"Event created successful, assigned id = {testEvent.Id}" : "Failed creating Event";
            Console.WriteLine(msg);
        }

        [Test]
        public void CanUpdateEvent()
        {
            var firstEvent = _service.SearchEvents(string.Empty).FirstOrDefault();
            Assert.NotNull(firstEvent);
            var testId = firstEvent.Id;
            firstEvent.Title = "Modified Test Event";
            Assert.DoesNotThrow(() =>
            {
                _service.CreateOrUpdateEvent(firstEvent);
            });
            Assert.That(firstEvent.Id==testId);
        }

        [Test]
        public void CanFindEventBasedOnTitleDescriptionLocation()
        {
            Console.WriteLine("Searching for events targeting title description and location, then adding event and redoing the search.");
            Console.WriteLine("Each search phrase should increase by 1. Found ( before / after ) added test event.");

            var foundTitleCount = _service.SearchEvents("banan").Count;
            var foundDescriptionCount = _service.SearchEvents("MiniGolf").Count;
            var foundLocationCount = _service.SearchEvents("Umeå").Count;

            _service.CreateOrUpdateEvent(
                new EventModel
                {
                    Title = "Banan ätar tävling ",
                    Description = "Vi träffas och spelar minigolf",
                    MeetingPlace = "Umeå",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now
                });
            var foundTitleCount2 = _service.SearchEvents("banan").Count;
            var foundDescriptionCount2 = _service.SearchEvents("MiniGolf").Count;
            var foundLocationCount2 = _service.SearchEvents("Umeå").Count;

            Assert.That(foundTitleCount2==foundTitleCount+1);
            Assert.That(foundDescriptionCount2 == foundDescriptionCount + 1);
            Assert.That(foundLocationCount2 == foundLocationCount + 1);

            
            Console.WriteLine($"Title : {foundTitleCount} / {foundTitleCount2} .");
            Console.WriteLine($"Description : {foundTitleCount} / {foundTitleCount2} .");
            Console.WriteLine($"Location : {foundTitleCount} / {foundTitleCount2} .");
        }

        [Test]
        public void GetAllEventsFilterByLocation()
        {
            var eventsCountBefore = _service.SearchEvents(string.Empty, "Umeå").Count;
            _service.CreateOrUpdateEvent(
                new EventModel
                {
                    Title = "Location Test Event",
                    Description = "Location Test Description",
                    MeetingPlace = "Umeå",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now
                });
            var eventCountAfter = _service.SearchEvents(String.Empty, "Umeå").Count;
            Assert.That(eventCountAfter==eventsCountBefore+1);
        }
    }

}