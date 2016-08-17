using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using EventBot.Entities.Models;
using EventBot.Entities.Service.Interfaces;
using EventBot.Entities.Service.Models;

namespace EventBot.Entities.Service
{
    public class EventServiceEf : IEventService
    {
        public void CreateOrUpdateEvent(EventModel model)
        {
            model.CreatedDate = model.Id == 0 ? DateTime.Now : model.CreatedDate;
            model.ModifiedDate = DateTime.Now;
            using (var db = new EventBotDb())
            {
                db.Events.AddOrUpdate(new Event
                {
                    Id = model.Id,
                    Title = model.Title,
                    Description = model.Description,
                    //Organiser
                    CreatedDate = model.CreatedDate,
                    ModifiedDate = model.ModifiedDate,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    IsCanceled = model.IsCanceled,
                    Image = new EventBotImage
                    {
                        Id = model.ImageId
                    },
                    MeatingPlace = model.MeatingPlace,
                    VisitCount = model.VisitCount
                });
                db.SaveChanges();
            }
        }

        public void JoinEvent(string userId, int eventId)
        {
            throw new NotImplementedException();
        }

        public ICollection<EventModel> SearchEvents(string query, string location = null)
        {
            throw new NotImplementedException();
        }

        public void CreateOrUpdateEventType(EventTypeModel model)
        {
            throw new NotImplementedException();
        }

        public ICollection<EventTypeModel> GetEventTypes()
        {
            throw new NotImplementedException();
        }

        public void SubscribeToEventType(string userId, int eventTypeId)
        {
            throw new NotImplementedException();
        }

        public void UnSubscribeFromEventType(string userId, int eventTypeId)
        {
            throw new NotImplementedException();
        }

        public ICollection<string> GetSubscribedEventTypeUserIds(ICollection<EventTypeModel> eventTypes)
        {
            throw new NotImplementedException();
        }
    }
}