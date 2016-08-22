using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBot.Entities.Models
{
    public class Notification
    {
        #region Properties

        public int Id { get; private set; }
        public DateTime DateTime { get; private set; }
        public NotificationType Type { get; private set; }
        public DateTime? OriginalStartDate { get; private set; }
        [Required]
        public Event Event { get; private set; }

        #endregion


        #region Constructors
        protected Notification()
        {

        }
        private Notification(NotificationType type, Event ev)
        {
            if (ev == null)
                throw new ArgumentException("Event == null");

            Type = type;
            Event = ev;
            DateTime = DateTime.Now;
        }
        #endregion

        #region Methods

        public static Notification EventCreated(Event ev)
        {
            return new Notification(NotificationType.EventCreated, ev);
        }

        public static Notification EventUpdated(Event newEvent, DateTime originalStartDate, DateTime originalEndDate, string originalContent)
        {
            var notification = new Notification(NotificationType.EventUpdated, newEvent);
            notification.OriginalStartDate = originalStartDate;

            return notification;
        }

        public static Notification EventCanceled(Event ev)
        {
            return new Notification(NotificationType.EventCanceled, ev);
        }

        #endregion
    }
}
