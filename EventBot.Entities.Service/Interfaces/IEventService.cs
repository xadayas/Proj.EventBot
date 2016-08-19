using System.Collections.Generic;
using EventBot.Entities.Service.Models;

namespace EventBot.Entities.Service.Interfaces
{
    public interface IEventService
    {
        void CreateOrUpdateEvent(EventModel model);
        EventModel GetEvent(int id);
        ICollection<EventModel> GetUserCreatedEvents(string userId); 
        void JoinEvent(string userId, int eventId);
        ICollection<EventModel> SearchEvents(string query,string location = null);
        void CreateOrUpdateEventType(EventTypeModel model);
        ICollection<EventTypeModel> GetEventTypes();
        void SubscribeToEventType(string userId,int eventTypeId);
        void UnSubscribeFromEventType(string userId, int eventTypeId);
        ICollection<string> GetSubscribedEventTypeUserIds(ICollection<EventTypeModel> eventTypes);
        byte[] GetImage(int imageId);
        void CreateImage(byte[] imageBytes);
    }
}
