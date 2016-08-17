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

        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public NotificationType Type { get; set; }
        public DateTime OriginalStartDate { get; set; }
        public DateTime OriginalEndDate { get; set; }
        public string OriginalContent { get; set; }
        [Required]
        public Event Event { get; set; }

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

        public static Notification UpdatedCreated(Event newEvent, DateTime originalStartDate, DateTime originalEndDate, string originalContent)
        {
            var notification = new Notification(NotificationType.EventUpdated, newEvent);
            notification.OriginalStartDate = originalStartDate;
            notification.OriginalEndDate = originalEndDate;
            notification.OriginalContent = originalContent;

            return notification;
        }

        public static Notification EventCanceled(Event ev)
        {
            return new Notification(NotificationType.EventCanceled, ev);
        }

        #endregion
    }
}
