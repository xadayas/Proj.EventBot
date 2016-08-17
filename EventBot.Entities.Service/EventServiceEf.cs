using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
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
                var newEvent = new Event
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
                    MeetingPlace = model.MeetingPlace,
                    VisitCount = model.VisitCount
                };
                db.Events.AddOrUpdate(newEvent);
                db.SaveChanges();
                model.Id = newEvent.Id;
            }
        }

        public void JoinEvent(string userId, int eventId)
        {
            using (var db = new EventBotDb())
            {
                var tempUser = db.Users.SingleOrDefault(w => w.Id == userId);
                if (tempUser == null) throw new InvalidOperationException("User not found.");
                var tempEvent = db.Events.SingleOrDefault(w => w.Id == eventId);
                if (tempEvent == null) throw new InvalidOperationException("Event not found.");
                var tempEventUser = db.EventUsers.SingleOrDefault(w => w.User.Id == userId && w.Event.Id == eventId);
                //TODO Return something if user already joined event ? For now just return.
                if (tempEventUser != null) return;

                tempEvent.Users.Add(new EventUser
                {
                    User = tempUser,
                    Event = tempEvent
                });
            }
        }

        public ICollection<EventModel> SearchEvents(string query, string location = null)
        {
            var queryLowerCase = query.ToLower();
            var queryEmpty = string.IsNullOrWhiteSpace(query);
            var locationEmpty = string.IsNullOrWhiteSpace(location);
            using (var db = new EventBotDb())
            {
                return db.Events.
                    Where(w => queryEmpty
                               || w.Title.ToLower().Contains(queryLowerCase)
                               || w.Description.ToLower().Contains(queryLowerCase)
                               || w.MeetingPlace.ToLower().Contains(queryLowerCase)
                               && (locationEmpty
                                   || w.MeetingPlace.ToLower()==location.ToLower()
                                   )
                    )
                    .Select(o => new
                    {
                        Id = o.Id,
                        Title = o.Title,
                        Description = o.Description,
                        CreatedDate = o.CreatedDate,
                        ModifiedDate = o.ModifiedDate,
                        MeetingPlace = o.MeetingPlace,
                        StartDate = o.StartDate,
                        EndDate = o.EndDate,
                        IsCanceled = o.IsCanceled,
                        ImageId = o.Image.Id,
                        VisitCount = o.VisitCount,
                        EventTypes = o.EventTypes,
                        UserId = o.Organiser.Id
                    }).ToArray()
                    .Select(s=>new EventModel
                    {
                        Id = s.Id,
                        Title = s.Title,
                        Description = s.Description,
                        CreatedDate = s.CreatedDate,
                        ModifiedDate = s.ModifiedDate,
                        MeetingPlace = s.MeetingPlace,
                        StartDate = s.StartDate,
                        EndDate = s.EndDate,
                        IsCanceled = s.IsCanceled,
                        ImageId = s.ImageId,
                        VisitCount = s.VisitCount,
                        EventTypes = s.EventTypes.Select(ss=>new EventTypeModel
                        {
                            Id = ss.Id,
                            Name = ss.Name
                        }).ToArray(),
                        UserId = s.UserId
                    }).ToArray();
            }
        }

        public void CreateOrUpdateEventType(EventTypeModel model)
        {
            using (var db = new EventBotDb())
            {
                // TODO Return something if EventType already exists. For now just return.
                if (model.Id == 0 && db.EventTypes.Any(w => w.Name == model.Name)) return;
                db.EventTypes.AddOrUpdate(new EventType
                {
                    Id = model.Id,
                    Name = model.Name
                });
                db.SaveChanges();
            }
        }

        public ICollection<EventTypeModel> GetEventTypes()
        {
            using (var db = new EventBotDb())
            {
                return db.EventTypes.Select(s =>
                    new EventTypeModel
                    {
                        Id = s.Id,
                        Name = s.Name
                    }).ToArray();
            }
        }

        public void SubscribeToEventType(string userId, int eventTypeId)
        {
            using (var db = new EventBotDb())
            {
                var user = db.Users.SingleOrDefault(w => w.Id == userId);
                if (user == null) throw new InvalidOperationException("User not found.");
                var eventType = db.EventTypes.SingleOrDefault(w => w.Id == eventTypeId);
                if (eventType == null) throw new InvalidOperationException("EventType not found.");
                user.EventTypeInterests.Add(eventType);
                db.SaveChanges();
            }
        }

        public void UnSubscribeFromEventType(string userId, int eventTypeId)
        {
            using (var db = new EventBotDb())
            {
                var user = db.Users.SingleOrDefault(w => w.Id == userId);
                if (user == null) throw new InvalidOperationException("User not found.");
                var eventType = user.EventTypeInterests.SingleOrDefault(w => w.Id == eventTypeId);
                if (eventType == null) throw new InvalidOperationException("EventType not found.");
                user.EventTypeInterests.Remove(eventType);
                db.SaveChanges();
            }
        }

        public ICollection<string> GetSubscribedEventTypeUserIds(ICollection<EventTypeModel> eventTypes)
        {
            var userIds = new List<string>();
            using (var db = new EventBotDb())
            {
                foreach (var eventTypeModel in eventTypes)
                {
                    var eventType = db.EventTypes.SingleOrDefault(w => w.Id == eventTypeModel.Id);
                    if (eventType == null) throw new InvalidOperationException("EventType not found.");
                    var tempIds =
                        db.Users.Where(w => w.EventTypeInterests.Contains(eventType))
                            .Select(s => s.Id).ToArray();
                    userIds.AddRange(tempIds);
                }
                return userIds.Distinct().ToArray();
            }
        }
    }
}