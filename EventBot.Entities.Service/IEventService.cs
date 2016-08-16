using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventBot.Entities.Models;
using EventBot.Entities.Service.Models;

namespace EventBot.Entities.Service
{
    public interface IEventService
    {
        void CreateOrUpdateEvent(EventModel model);
        void JoinEvent(string userId, int eventId);
        ICollection<EventModel> SearchEvents(string query,string location = null);
        void CreateOrUpdateEventType(EventTypeModel model);
        ICollection<EventTypeModel> GetEventTypes();
        void SubscribeToEventType(string userId,int eventTypeId);
        void UnSubscribeFromEventType(string userId, int eventTypeId);
        ICollection<string> GetSubscribedEventTypeUserIds(ICollection<EventTypeModel> eventTypes);
    }
}
