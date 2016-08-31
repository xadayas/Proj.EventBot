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

            using (var db = new EventBotDb())
            {



                var testEvent = new EventModel
                {
                    UserId = db.Users.First().Id,
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
                Assert.That(testEvent.Id != 0);
                string msg = testEvent.Id != 0
                    ? $"Event created successful, assigned id = {testEvent.Id}"
                    : "Failed creating Event";
                Console.WriteLine(msg);
            }
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
            Assert.That(firstEvent.Id == testId);
        }

        [Test]
        public void CanFindEventBasedOnTitleDescriptionLocation()
        {
            Console.WriteLine("Searching for events targeting title description and location, then adding event and redoing the search.");
            Console.WriteLine("Each search phrase should increase by 1. Found ( before / after ) added test event.");

            var foundTitleCount = _service.SearchEvents("banan").Count;
            var foundDescriptionCount = _service.SearchEvents("MiniGolf").Count;
            var foundLocationCount = _service.SearchEvents("Umeå").Count;
            var userid = string.Empty;
            using (var db = new EventBotDb()) userid = db.Users.First().Id;
            _service.CreateOrUpdateEvent(
                new EventModel
                {
                    UserId = userid,
                    Title = "Banan ätar tävling ",
                    Description = "Vi träffas och spelar minigolf",
                    MeetingPlace = "Umeå",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now
                });
            var foundTitleCount2 = _service.SearchEvents("banan").Count;
            var foundDescriptionCount2 = _service.SearchEvents("MiniGolf").Count;
            var foundLocationCount2 = _service.SearchEvents("Umeå").Count;

            Assert.That(foundTitleCount2 == foundTitleCount + 1);
            Assert.That(foundDescriptionCount2 == foundDescriptionCount + 1);
            Assert.That(foundLocationCount2 == foundLocationCount + 1);


            Console.WriteLine($"Title : {foundTitleCount} / {foundTitleCount2} .");
            Console.WriteLine($"Description : {foundTitleCount} / {foundTitleCount2} .");
            Console.WriteLine($"Location : {foundTitleCount} / {foundTitleCount2} .");
        }

        [Test]
        public void GetAllEventsFilterByLocation()
        {
            var eventsCountBefore = _service.SearchEvents(string.Empty, location:"Umeå").Count;
            var userid = string.Empty;
            using (var db = new EventBotDb()) userid = db.Users.First().Id;
            _service.CreateOrUpdateEvent(
                new EventModel
                {
                    UserId = userid,
                    Title = "Location Test Event",
                    Description = "Location Test Description",
                    MeetingPlace = "Umeå",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now
                });
            var eventCountAfter = _service.SearchEvents(String.Empty, location:"Umeå").Count;
            Assert.That(eventCountAfter == eventsCountBefore + 1);
        }

        [Test]
        public void CanCreateAndUpdateEventType()
        {
            Assert.DoesNotThrow(() =>
            {

                var testEventType = _service.GetEventTypes().FirstOrDefault(w => w.Name == "Pokemon");
                if (testEventType == null) testEventType = new EventTypeModel
                {
                    Name = "Pokemon"
                };
                _service.CreateOrUpdateEventType(testEventType);
                Assert.That(_service.GetEventTypes().Count(w => w.Name == "Pokemon") == 1);
                testEventType.Name = "Pokemon2";
                _service.CreateOrUpdateEventType(testEventType);
                Assert.That(_service.GetEventTypes().Count(w => w.Name == "Pokemon") == 0);
                Assert.That(_service.GetEventTypes().Count(w => w.Name == "Pokemon2") == 1);
                testEventType.Name = "Pokemon";
                _service.CreateOrUpdateEventType(testEventType);
            });
        }
        [Test]
        public void ImageTest()
        {
            var img = _service.GetImage(6);
            Console.WriteLine("Image2:");
            var hex = String.Join(String.Empty, Array.ConvertAll(img, x => x.ToString("X2")));
            Console.WriteLine(hex);
        }
    }

}