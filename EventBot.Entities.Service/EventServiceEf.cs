using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using EventBot.Entities.Models;
using EventBot.Entities.Service.Interfaces;
using EventBot.Entities.Service.Models;
using System.Data.Entity;

namespace EventBot.Entities.Service
{
    public class EventServiceEf : IEventService
    {
        public void CreateOrUpdateEvent(EventModel model)
        {
            using (var db = new EventBotDb())
            {
                var even = db.Events.FirstOrDefault(e => e.Id == model.Id) ?? new Event();
                even.Id = model.Id;
                even.Title = model.Title;
                even.Description = model.Description;
                even.Organiser = db.Users.Single(u => u.Id == model.UserId);
                even.CreatedDate = even.Id == 0 ? DateTime.Now : even.CreatedDate;
                even.ModifiedDate = DateTime.Now;
                even.StartDate = model.StartDate;
                even.EndDate = model.EndDate;
                even.IsCanceled = model.IsCanceled;
                even.Image = model.ImageId == 0 ? null : db.Images.FirstOrDefault(w => w.Id == model.ImageId);
                even.MeetingPlace = model.MeetingPlace;
                even.VisitCount = model.VisitCount;
                db.Events.AddOrUpdate(even);
                db.SaveChanges();
                model.Id = even.Id;
            }
        }


        public EventModel GetEvent(int id)
        {
            using (var db = new EventBotDb())
            {
                var ev = db.Events.Where(e => e.Id == id);
                if (ev == null)
                    return null;
                return ev.Select(e =>  new EventModel()
                {
                    Id = e.Id,
                    CreatedDate = e.CreatedDate,
                    EndDate = e.EndDate,
                    Description = e.Description,
                    ImageId = e.Image == null ? 0 : e.Image.Id,
                    IsCanceled = e.IsCanceled,
                    MeetingPlace = e.MeetingPlace,
                    ModifiedDate = e.ModifiedDate,
                    StartDate = e.StartDate,
                    Title = e.Title,
                }).FirstOrDefault();
            }
        }

        public ICollection<EventModel> GetUserCreatedEvents(string userId)
        {
            using (var db = new EventBotDb())
            {
               return db.Events.Where(e => e.Organiser.Id == userId)
                    .Select(@event => new EventModel
                    {
                        Id = @event.Id,
                        Title = @event.Title,
                        Description = @event.Description,
                        CreatedDate = @event.CreatedDate,
                        ModifiedDate = @event.ModifiedDate,
                        MeetingPlace = @event.MeetingPlace,
                        StartDate = @event.StartDate,
                        EndDate = @event.EndDate,
                        IsCanceled = @event.IsCanceled,
                        ImageId = @event.Image == null ? 0 : @event.Image.Id,
                        VisitCount = @event.VisitCount,
                        EventTypes = @event.EventTypes.Select(eventType => new EventTypeModel
                        {
                            Id = eventType.Id,
                            Name = eventType.Name
                        }).ToList(),
                        UserId = @event.Organiser.Id
                    }).ToList();
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
                db.SaveChanges();
            }
        }

        public void LeaveEvent(string userId, int eventId)
        {
            using (var db = new EventBotDb())
            {
                var attandee = db.EventUsers.SingleOrDefault(s => s.Event.Id == eventId && s.User.Id == userId);

                if (attandee == null)
                    throw new InvalidOperationException("No particpant is found.");
                db.EventUsers.Remove(attandee);
                db.SaveChanges();
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
                                   || w.MeetingPlace.ToLower() == location.ToLower()
                                   )
                    )
                    .Select(o => new
                    {
                        o.Id,
                        o.Title,
                        o.Description,
                        o.CreatedDate,
                        o.ModifiedDate,
                        o.MeetingPlace,
                        o.StartDate,
                        o.EndDate,
                        o.IsCanceled,
                        ImageId = o.Image == null ? 0 : o.Image.Id,
                        o.VisitCount,
                        o.EventTypes,
                        UserId = o.Organiser.Id
                    }).ToArray()
                    .Select(s => new EventModel
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
                        EventTypes = s.EventTypes.Select(ss => new EventTypeModel
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
                var eventType = new EventType
                {
                    Id = model.Id,
                    Name = model.Name
                };
                db.EventTypes.AddOrUpdate(eventType);
                db.SaveChanges();
                model.Id = eventType.Id;
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

        public byte[] GetImage(int imageId)
        {
            using (var db = new EventBotDb())
            {
                var image = db.Images.SingleOrDefault(s => s.Id == imageId);
                if(image==null)throw new InvalidOperationException("Image not found");
                return image.ImageBytes;
            }
        }

        public int CreateImage(byte[] imageBytes)
        {
            var image = new EventBotImage
            {
                ImageBytes = imageBytes
            };
            using (var db = new EventBotDb())
            {
                db.Images.Add(image);
                db.SaveChanges();
            }
            return image.Id;
        }

        public IEnumerable<Notification> GetNewNotificationsFor(string userId)
        {
            using (var db = new EventBotDb())
            {
                return db.UserNotifications
                    .Where(u => u.UserId == userId && !u.IsRead)
                    .Select(n => n.Notification)
                    .Include(o => o.Event.Organiser)
                    .ToList();                  
            }
        }

        public void MarkNotificationAsRead(string userId)
        {
            using (var db = new EventBotDb())
            {
                var notifications = db.UserNotifications
                    .Where(user => user.UserId == userId && !user.IsRead)
                    .ToList();

                notifications.ForEach(notification => notification.NotificationsRead());
                db.SaveChanges();
            }
        }

        public bool CheckParticipant(string userId, int eventId)
        {
            using (var db = new EventBotDb())
            {
                return db.EventUsers.Any(s => s.User.Id == userId && s.Event.Id == eventId);
            }
        }

        public void ChangeName(string userId, string name)
        {
            using (var db = new EventBotDb())
            {
                var user = db.Users.FirstOrDefault(u => u.Id == userId);
                if (user == null || name == null) 
                    throw new InvalidOperationException("User not found, or name string empty.");

                user.Name = name;
                db.Users.AddOrUpdate(user);
                db.SaveChanges();
            }
        }

        public string GetName(string userId)
        {
            using (var db = new EventBotDb())
            {
                var user = db.Users.FirstOrDefault(u => u.Id == userId);

                if (user == null)
                    throw new InvalidOperationException("No user found.");

                return user.Name;
            }
        }
    }
}

