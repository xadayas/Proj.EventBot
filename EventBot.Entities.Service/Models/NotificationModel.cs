using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventBot.Entities.Models;

namespace EventBot.Entities.Service.Models
{
    public class NotificationModel
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public NotificationType EventType { get; set; }
        public DateTime? OriginalStartDate { get; set; }
        public string EventName { get; set; }
        public int EventId { get; set; }
        //public virtual User User { get; set; }
        public bool IsRead { get; set; }
    }
}
