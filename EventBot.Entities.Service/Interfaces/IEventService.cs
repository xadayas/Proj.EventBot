using System.Collections.Generic;
using EventBot.Entities.Service.Models;
using EventBot.Entities.Models;

namespace EventBot.Entities.Service.Interfaces
{
    public interface IEventService
    {
        void CreateOrUpdateEvent(EventModel model);
        EventModel GetEvent(int id);
        ICollection<EventModel> GetUserCreatedEvents(string userId);
        ICollection<EventModel> GetAllUpComingEvents();
        ICollection<EventModel> GetAllUpcomingEventsFor(string userId);

        void JoinEvent(string userId, int eventId);
        void LeaveEvent(string userId, int eventId);

        ICollection<EventModel> SearchEvents(string query,string location = null);
        void CreateOrUpdateEventType(EventTypeModel model);
        ICollection<EventTypeModel> GetEventTypes();

        void SubscribeToEventType(string userId,int eventTypeId);
        void UnSubscribeFromEventType(string userId, int eventTypeId);
        ICollection<string> GetSubscribedEventTypeUserIds(ICollection<EventTypeModel> eventTypes);

        byte[] GetImage(int imageId);
        int CreateImage(byte[] imageBytes);

        IEnumerable<NotificationModel> GetNewNotificationsFor(string userId);
        void MarkAllNotificationsAsRead(string userId);
        void MarkNotificationAsRead(int id,string userId);

        bool CheckParticipant(string userId, int eventId);

        void ChangeName(string userId, string name);
        string GetName(string userId);
    }
}
