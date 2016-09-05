using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using EventBot.Entities.Models;
using EventBot.Entities.Service.Interfaces;
using EventBot.Entities.Service.Models;
using System.Data.Entity;
using System.Drawing;
using System.IO;

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
                var originalDateTime = model.StartDate;
                if (even.Id != 0)
                {
                    notificationType = NotificationType.EventUpdated;
                    originalDateTime = even.StartDate;
                }
                even.Id = model.Id;
                even.Title = model.Title;
                even.Description = model.Description;
                even.Organiser = db.Users.Single(u => u.Id == model.UserId);
                even.CreatedDate = even.Id == 0 ? DateTime.Now : even.CreatedDate;
                even.ModifiedDate = DateTime.Now;
                even.StartDate = model.StartDate;
                even.EndDate = model.EndDate;
                even.IsCanceled = model.IsCanceled;
                even.ParticipationCost = model.ParticipationCost;
                even.MaxAttendees = model.MaxAttendees;
                even.Image = model.ImageId == 0 ? null : db.Images.FirstOrDefault(w => w.Id == model.ImageId);
                even.MeetingPlace = model.MeetingPlace;
                even.Location = new Location
                {
                    Id = model.Location.Id,
                    Latitude = model.Location.Latitude,
                    Longitude = model.Location.Longitude,
                    Altitude = model.Location.Altitude,
                    Name = model.Location.Name,
                    Country = model.Location.Country,
                    City = model.Location.City
                };
                if (even.EventTypes == null) even.EventTypes = new List<EventType>();
                if (model.EventTypes != null)
                {
                    even.EventTypes.Clear();
                    foreach (var eventTypeModel in model.EventTypes)
                    {
                        var eventTypeToAdd = db.EventTypes.FirstOrDefault(f => f.Id == eventTypeModel.Id);
                        if (eventTypeToAdd != null) even.EventTypes.Add(eventTypeToAdd);
                        else
                        {
                            var newEventType = new EventType { Name = eventTypeModel.Name };
                            even.EventTypes.Add(newEventType);
                        }
                    }
                }
                if (even.IsCanceled)
                    notificationType = NotificationType.EventCanceled;
                db.Events.AddOrUpdate(even);
                if (notificationType == NotificationType.EventUpdated || notificationType == NotificationType.EventCanceled)
                    CreateEventNotification(db, even, notificationType, originalDateTime);
                db.SaveChanges();
                model.Id = even.Id;
            }
        }


        public EventModel GetEvent(int id)
        {
            using (var db = new EventBotDb())
            {
                var ev = db.Events.Include(p => p.Location).Include(p => p.Organiser).Include(p => p.Image).Include(p => p.Users).SingleOrDefault(s => s.Id == id);
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
                    ParticipationCost = ev.ParticipationCost,
                    MaxAttendees = ev.MaxAttendees,
                    UserCount = ev.Users.Count,
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
                        Name = ev.Location.Name,
                        Country = ev.Location.Country,
                        City = ev.Location.City
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
                         MaxAttendees = @event.MaxAttendees,
                         ParticipationCost = @event.ParticipationCost,
                         UserCount = @event.Users.Count,
                         Location = new LocationModel
                         {
                             Id = @event.Location.Id,
                             Latitude = @event.Location.Latitude,
                             Longitude = @event.Location.Longitude,
                             Altitude = @event.Location.Altitude,
                             Name = @event.Location.Name,
                             Country = @event.Location.Country,
                             City = @event.Location.City
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
                         MaxAttendees = @event.MaxAttendees,
                         ParticipationCost = @event.ParticipationCost,
                         UserCount = @event.Users.Count,
                         Location = new LocationModel
                         {
                             Id = @event.Location.Id,
                             Latitude = @event.Location.Latitude,
                             Longitude = @event.Location.Longitude,
                             Altitude = @event.Location.Altitude,
                             Name = @event.Location.Name,
                             Country = @event.Location.Country,
                             City = @event.Location.City
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

        public ICollection<EventModel> GetAllUpcomingEventsFor(string userId)
        {
            using (var db = new EventBotDb())
            {
                var user = db.Users.FirstOrDefault(e => e.Id == userId);

                //Fullösning.se tillsvidare...
                var listInts = new List<int>();
                foreach (var whot in user.AttendingEvents)
                {
                    listInts.Add(whot.Id);
                }
                var listEvent = new List<Event>();
                foreach (var item in listInts)
                {
                    listEvent.Add(db.Events.Include(p => p.Location).Include(p => p.Organiser).Include(p => p.Image).SingleOrDefault(s => s.Id == item));
                }

                var list = listEvent.Where(e => e.StartDate > DateTime.Now && !e.IsCanceled)
                     .Select(@event => new EventModel
                     {
                         Id = @event.Id,
                         Title = @event.Title,
                         Description = @event.Description,
                         CreatedDate = @event.CreatedDate,
                         ModifiedDate = @event.ModifiedDate,
                         MeetingPlace = @event.MeetingPlace,
                         MaxAttendees = @event.MaxAttendees,
                         ParticipationCost = @event.ParticipationCost,
                         UserCount = @event.Users.Count,
                         Location = new LocationModel
                         {
                             Id = @event.Location.Id,
                             Latitude = @event.Location.Latitude,
                             Longitude = @event.Location.Longitude,
                             Altitude = @event.Location.Altitude,
                             Name = @event.Location.Name,
                             Country = @event.Location.Country,
                             City = @event.Location.City
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
                     }).ToList().OrderBy(p => p.StartDate);

                return list.ToList();
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
                var tempOwner = db.Events.Where(d => d.Id == eventId).Select(e => e.Organiser).Single();

                //TODO Return something if user already joined event ? For now just return.
                if (tempEvent == null || tempUser == null) return;

                if (!tempEvent.Users.Contains(tempUser))
                {
                    tempEvent.Users.Add(tempUser);
                    CreateEventNotificationForUser(db, tempEvent, tempUser, NotificationType.EventJoined, tempEvent.StartDate);
                    if (tempOwner.Id != tempUser.Id)
                        CreateEventNotificationForUser(db, tempEvent, tempOwner, NotificationType.EventUserHasJoined, tempEvent.StartDate);
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
                var tempOwner = db.Events.Where(d => d.Id == eventId).Select(e => e.Organiser).Single();
                if (attandee != null)
                {
                    CreateEventNotificationForUser(db, tempEvent, attandee, NotificationType.EventLeaved, tempEvent.StartDate);
                    if (tempOwner.Id != attandee.Id)
                        CreateEventNotificationForUser(db, tempEvent, tempOwner, NotificationType.EventUserHasLeaved, tempEvent.StartDate);
                    tempEvent.Users.Remove(attandee);
                    db.SaveChanges();
                }
            }
        }

        public ICollection<EventModel> SearchEvents(string query, int maxCost = -1, int minFreePlaces = 0, EventSortBy sortBy = EventSortBy.Popularity, LocationModel location = null, double maxDistance = 0,int modulus=1)
        {
            if (query == null) return null;
            var queryLowerCase = query.ToLower();
            var queryEmpty = string.IsNullOrWhiteSpace(query);
            var locationEmpty = location == null;

            using (var db = new EventBotDb())
            {
                //Search
                var eventsMatchingQuery = db.Events
                    .Where(w => w.StartDate > DateTime.Now)
                    .Where(w => (queryEmpty
                               || w.Title.ToLower().Contains(queryLowerCase)
                               || w.Description.ToLower().Contains(queryLowerCase)
                               || w.MeetingPlace.ToLower().Contains(queryLowerCase)
                               || w.EventTypes.Any(eventtype => eventtype.Name.ToLower().Contains(queryLowerCase)))
                               && (maxCost < 0
                                   || w.ParticipationCost <= maxCost)
                                   && (w.MaxAttendees == 0 || ((w.MaxAttendees - w.Users.Count) >= minFreePlaces))
                    );
                IQueryable<Event> eventsMatchingQueryOrdered;
                switch (sortBy)
                {
                    case EventSortBy.Price:
                        eventsMatchingQueryOrdered = eventsMatchingQuery.OrderBy(p => p.ParticipationCost);
                        break;
                    case EventSortBy.Date:
                        eventsMatchingQueryOrdered = eventsMatchingQuery.OrderBy(p => p.StartDate);
                        break;
                    case EventSortBy.Title:
                        eventsMatchingQueryOrdered = eventsMatchingQuery.OrderBy(p => p.Title);
                        break;
                    case EventSortBy.Popularity:
                        eventsMatchingQueryOrdered = eventsMatchingQuery.OrderByDescending(p => p.VisitCount);
                        break;
                    default:
                        eventsMatchingQueryOrdered = eventsMatchingQuery;
                        break;
                }


                var result =  eventsMatchingQueryOrdered.Select(o => new
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
                    o.MaxAttendees,
                    o.ParticipationCost,
                    ImageId = o.Image == null ? 0 : o.Image.Id,
                    o.VisitCount,
                    o.EventTypes,
                    UserCount = o.Users.Count,
                    UserId = o.Organiser.Id
                }).ToArray().Where(w=>(maxDistance == 0||location.Latitude==0||location.Longitude==0 || w.Location.DistanceTo(location) < maxDistance))
                .Select(s => new EventModel
                {
                    Id = s.Id,
                    Title = s.Title,
                    Description = s.Description,
                    CreatedDate = s.CreatedDate,
                    ModifiedDate = s.ModifiedDate,
                    MeetingPlace = s.MeetingPlace,
                    MaxAttendees = s.MaxAttendees,
                    ParticipationCost = s.ParticipationCost,
                    UserCount = s.UserCount,
                    Location = new LocationModel
                    {
                        Id = s.Location.Id,
                        Latitude = s.Location.Latitude,
                        Longitude = s.Location.Longitude,
                        Altitude = s.Location.Altitude,
                        Name = s.Location.Name,
                        Country = s.Location.Country,
                        City = s.Location.City
                    },
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    IsCanceled = s.IsCanceled,
                    ImageId = s.ImageId,
                    VisitCount = s.VisitCount,
                    DistanceFromClient=s.Location.DistanceTo(location),
                    EventTypes = s.EventTypes.Select(ss => new EventTypeModel
                    {
                        Id = ss.Id,
                        Name = ss.Name
                    }).ToArray(),
                    UserId = s.UserId
                });

                if (sortBy == EventSortBy.Distance) result= result.OrderBy(o => o.DistanceFromClient);
                var materializedResult = result.ToArray();
                // modulus
                if (modulus != 1 && materializedResult.Length > modulus)
                {
                    var count = materializedResult.Length-(materializedResult.Length%modulus);
                    materializedResult = materializedResult.Take(count).ToArray();
                }
                return materializedResult;
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
                return db.EventTypes.Select(s => new EventTypeModel
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
                    var tempIds = db.Users.Where(w => w.EventTypeInterests.Contains(eventType)).Select(s => s.Id).ToArray();
                    userIds.AddRange(tempIds);
                }
                return userIds.Distinct().ToArray();
            }
        }

        #endregion

        #region Images

        public byte[] GetImageLarge(int imageId)
        {
            using (var db = new EventBotDb())
            {
                var image = db.Images.SingleOrDefault(s => s.Id == imageId);
                if (image == null) throw new InvalidOperationException("Image not found");
                return image.ImageBytesLarge;
            }
        }
        public byte[] GetImageThumb(int imageId)
        {
            using (var db = new EventBotDb())
            {
                var image = db.Images.SingleOrDefault(s => s.Id == imageId);
                if (image == null) throw new InvalidOperationException("Image not found");
                return image.ImageBytesThumb;
            }
        }
        public int CreateImage(byte[] imageBytes)
        {
            Image imgFromBytesOriginal;
            using (var ms = new MemoryStream(imageBytes))
            {
                imgFromBytesOriginal = Image.FromStream(ms);
            }
            var largeImage = ImageHelpers.FixedSize(imgFromBytesOriginal, 300, 300, true);
            var thumbnailImage = ImageHelpers.FixedSize(imgFromBytesOriginal, 100, 100, true);

            var largeStream = new MemoryStream();
            largeImage.Save(largeStream, System.Drawing.Imaging.ImageFormat.Png);

            var thumbStream = new MemoryStream();
            thumbnailImage.Save(thumbStream, System.Drawing.Imaging.ImageFormat.Png);

            var image = new EventBotImage
            {
                ImageBytesLarge = largeStream.ToArray(),
                ImageBytesThumb = thumbStream.ToArray()
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
                return db.Notifications.Where(n => n.User.Id == userId && !n.IsRead).Select(n => new NotificationModel()
                {
                    DateTime = n.DateTime,
                    Id = n.Id,
                    EventName = n.Event.Title,
                    IsRead = n.IsRead,
                    OriginalStartDate = n.OriginalStartDate,
                    StartDate = n.StartDate,
                    EventType = n.Type,
                    EventId = n.Event.Id,
                }).ToList();
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

        private void CreateEventNotification(EventBotDb db, Event e, NotificationType type, DateTime originalDateTime)
        {
            var eventUsers = e.Users;
            foreach (var eventUser in eventUsers)
            {
                db.Notifications.AddOrUpdate(new Notification()
                {
                    DateTime = DateTime.Now,
                    Event = e,
                    StartDate = e.StartDate,
                    OriginalStartDate = originalDateTime,
                    Type = type,
                    User = eventUser
                });
            }
        }

        private void CreateEventNotificationForUser(EventBotDb db, Event e, User user, NotificationType type, DateTime originalDateTime)
        {
            db.Notifications.AddOrUpdate(new Notification()
            {
                DateTime = DateTime.Now,
                Event = e,
                StartDate = e.StartDate,
                OriginalStartDate = originalDateTime,
                Type = type,
                User = user
            });
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

        public void AddVisitorToEvent(int id)
        {
            using (var db = new EventBotDb())
            {
                var ev = db.Events.SingleOrDefault(s => s.Id == id);
                if (ev != null) ev.VisitCount++;
                db.SaveChanges();
            }
        }
    }
}

