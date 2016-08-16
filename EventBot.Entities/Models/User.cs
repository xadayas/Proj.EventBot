using System.Collections.Generic;

namespace EventBot.Entities.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public EventBotImage Image { get; set; }
        public bool IsCompany { get; set; }
        public virtual ICollection<EventType> EventTypeInterests { get; set; }
        public virtual ICollection<User> FollowingUsers { get; set; }
        public virtual ICollection<EventUser> Events { get; set; }
    }
}