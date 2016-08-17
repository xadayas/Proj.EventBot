using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EventBot.Entities.Models
{
    public class User : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public string Name { get; set; }
        public EventBotImage Image { get; set; }
        public bool IsCompany { get; set; }
        public virtual ICollection<EventType> EventTypeInterests { get; set; }
        public virtual ICollection<User> FollowingUsers { get; set; }
        public virtual ICollection<EventUser> Events { get; set; }
        public virtual ICollection<UserNotifications> Notifications { get; set; }

        public void Notify(Notification notification)
        {
            Notifications.Add(new UserNotifications(this, notification));
        }
    }
}