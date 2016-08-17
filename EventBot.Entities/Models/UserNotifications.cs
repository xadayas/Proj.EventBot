using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBot.Entities.Models
{
    public class UserNotifications
    {
        #region Properties

        public int Id { get; set; }
        public string UserId { get; set; }
        public int NotificationId { get; set; }
        public User User { get; set; }
        public Notification Notification { get; set; }
        public bool IsRead { get; set; }

        #endregion

        #region Constructors

        protected UserNotifications()
        {

        }

        public UserNotifications(User user, Notification notification)
        {
            if (user == null)
                throw new ArgumentException("User == null");
            if (notification == null)
                throw new ArgumentException("Notification == null");

            UserId = user.Id;
            User = user;
            Notification = notification;
            NotificationId = notification.Id;

        }

        #endregion


        #region Methods

        public void NotificationsRead()
        {
            IsRead = true;
        }

        #endregion
    }
}
