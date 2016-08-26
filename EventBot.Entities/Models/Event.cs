using System;
using System.Collections.Generic;

namespace EventBot.Entities.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public User Organiser { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public long VisitCount { get; set; }
        public string MeetingPlace { get; set; }
        public Location Location { get; set; }
        public EventBotImage Image { get; set; }
        public bool IsCanceled { get; set; }
        public virtual ICollection<EventType> EventTypes { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}