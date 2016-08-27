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
        #region get and update events
        public void CreateOrUpdateEvent(EventModel model)
        {
            using (var db = new EventBotDb())
            {
                var notificationType = NotificationType.EventCreated;
                var even = db.Events.FirstOrDefault(e => e.Id == model.Id) ?? new Event();
                if (even.Id != 0)
                    notificationType = NotificationType.EventUpdated;
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
                even.Location = new Location
                {
                    Id = model.Location.Id,
                    Latitude = model.Location.Latitude,
                    Longitude = model.Location.Longitude,
                    Altitude = model.Location.Altitude,
                    Name = model.Location.Name
                };
                if (even.EventTypes == null) even.EventTypes = new List<EventType>();
                if (model.EventTypes != null)
                    foreach (var eventTypeModel in model.EventTypes)
                    {
                        var eventTypeToAdd = db.EventTypes.FirstOrDefault(f => f.Id == eventTypeModel.Id);
                        if (eventTypeToAdd != null) even.EventTypes.Add(eventTypeToAdd);
                    }
                if (even.IsCanceled)
                    notificationType = NotificationType.EventCanceled;
                db.Events.AddOrUpdate(even);
                if (notificationType == NotificationType.EventUpdated || notificationType == NotificationType.EventCanceled)
                    CreateEventNotification(db, even, notificationType);
                db.SaveChanges();
                model.Id = even.Id;
            }
        }


        public EventModel GetEvent(int id)
        {
            using (var db = new EventBotDb())
            {
                var ev = db.Events.Include(p => p.Location).Include(p => p.Organiser).Include(p => p.Image).SingleOrDefault(s => s.Id == id);
                if (ev == null)
                    return null;
                var rEvent = new EventModel
                {
                    Id = ev.Id,
                    CreatedDate = ev.CreatedDate,
                    EndDate = ev.EndDate,
                    Description = ev.Description,
                    ImageId = ev.Image?.Id ?? 0,
                    IsCanceled = ev.IsCanceled,
                    MeetingPlace = ev.MeetingPlace,
                    ModifiedDate = ev.ModifiedDate,
                    StartDate = ev.StartDate,
                    Title = ev.Title,
                    UserId = ev.Organiser?.Id ?? "",
                    VisitCount = ev.VisitCount,
                    EventTypes = ev.EventTypes.Select(s => new EventTypeModel
                    {
                        Id = s.Id,
                        Name = s.Name
                    }).ToArray()
                };

                if (ev.Location != null)
                {
                    rEvent.Location = new LocationModel
                    {
                        Id = ev.Location.Id,
                        Latitude = ev.Location.Latitude,
                        Longitude = ev.Location.Longitude,
                        Altitude = ev.Location.Altitude,
                        Name = ev.Location.Name
                    };
                }
                return rEvent;
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
                         Location = new LocationModel
                         {
                             Id = @event.Location.Id,
                             Latitude = @event.Location.Latitude,
                             Longitude = @event.Location.Longitude,
                             Altitude = @event.Location.Altitude,
                             Name = @event.Location.Name
                         },
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

        public ICollection<EventModel> GetAllUpComingEvents()
        {
            using (var db = new EventBotDb())
            {
                return db.Events.Where(e => e.StartDate > DateTime.Now && !e.IsCanceled)
                     .Select(@event => new EventModel
                     {
                         Id = @event.Id,
                         Title = @event.Title,
                         Description = @event.Description,
                         CreatedDate = @event.CreatedDate,
                         ModifiedDate = @event.ModifiedDate,
                         MeetingPlace = @event.MeetingPlace,
                         Location = new LocationModel
                         {
                             Id = @event.Location.Id,
                             Latitude = @event.Location.Latitude,
                             Longitude = @event.Location.Longitude,
                             Altitude = @event.Location.Altitude,
                             Name = @event.Location.Name
                         },
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

                //TODO Return something if user already joined event ? For now just return.
                if (tempEvent == null || tempUser == null) return;

                if (!tempEvent.Users.Contains(tempUser))
                {
                    tempEvent.Users.Add(tempUser);
                    db.SaveChanges();
                }
            }
        }

        public void LeaveEvent(string userId, int eventId)
        {
            using (var db = new EventBotDb())
            {
                var tempEvent = db.Events.SingleOrDefault(e => e.Id == eventId);
                if (tempEvent == null)
                    throw new InvalidOperationException("Event not found.");
                var attandee = tempEvent.Users.SingleOrDefault(u => u.Id == userId);

                if (attandee != null)
                {
                    tempEvent.Users.Remove(attandee);
                    db.SaveChanges();
                }
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
                        o.Location,
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
                        Location = new LocationModel
                        {
                            Id = s.Location.Id,
                            Latitude = s.Location.Latitude,
                            Longitude = s.Location.Longitude,
                            Altitude = s.Location.Altitude,
                            Name = s.Location.Name
                        },
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
        #endregion
        #region eventtype
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
        #endregion
        #region Images
        public byte[] GetImage(int imageId)
        {
            using (var db = new EventBotDb())
            {
                var image = db.Images.SingleOrDefault(s => s.Id == imageId);
                if (image == null) throw new InvalidOperationException("Image not found");
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
        #endregion
        #region Notifications
        public IEnumerable<NotificationModel> GetNewNotificationsFor(string userId)
        {
            using (var db = new EventBotDb())
            {
                return db.Notifications
                    .Where(n => n.User.Id == userId && !n.IsRead)
                    .Select(n => new NotificationModel()
                    {
                        DateTime = n.DateTime,
                        Id = n.Id,
                        EventName = n.Event.Title,
                        IsRead = n.IsRead,
                        OriginalStartDate = n.OriginalStartDate,
                        EventType = n.Type,
                        EventId = n.Event.Id,

                    })
                    .ToList();
            }
        }
        public void MarkAllNotificationsAsRead(string userId)
        {
            //TODO världens jävla fullösning, men vanliga fungerar inte...
            var notIds = new List<int>();
            using (var db = new EventBotDb())
            {
                notIds = db.Notifications.Where(n => !n.IsRead && n.User.Id == userId).Select(n => n.Id).ToList();
            }
            foreach (var notId in notIds)
            {
                MarkNotificationAsRead(notId, userId);
            }
        }

        public void MarkNotificationAsRead(int id, string userId)
        {
            //TODO världens jävla fullösning, men vanliga fungerar inte...
            using (var db = new EventBotDb())
            {
                var user = db.Notifications.FirstOrDefault(n => n.Id == id && n.IsRead == false).User;
                var date = db.Notifications.FirstOrDefault(n => n.Id == id && n.IsRead == false).DateTime;
                var ev = db.Notifications.FirstOrDefault(n => n.Id == id && n.IsRead == false).Event;
                var or = db.Notifications.FirstOrDefault(n => n.Id == id && n.IsRead == false).OriginalStartDate;
                var type = db.Notifications.FirstOrDefault(n => n.Id == id && n.IsRead == false).Type;
                var result = db.Notifications.FirstOrDefault(n => n.Id == id && n.IsRead == false && n.User.Id == userId);
                if (result != null)
                {
                    result.User = user;
                    result.DateTime = date;
                    result.Event = ev;
                    result.OriginalStartDate = or;
                    result.Type = type;
                    result.IsRead = true;

                    db.SaveChanges();
                }
            }
        }
        private void CreateEventNotification(EventBotDb db, Event e, NotificationType type)
        {
            var eventUsers = e.Users;
            foreach (var eventUser in eventUsers)
            {
                db.Notifications.AddOrUpdate(new Notification()
                {
                    DateTime = DateTime.Now,
                    Event = e,
                    OriginalStartDate = e.StartDate,
                    Type = type,
                    User = eventUser
                });
            }
        }

        #endregion
        public bool CheckParticipant(string userId, int eventId)
        {
            using (var db = new EventBotDb())
            {
                var tempEvent = db.Events.SingleOrDefault(e => e.Id == eventId);
                return tempEvent.Users.Any(s => s.Id == userId);
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

