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
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public NotificationType Type { get; set; }
        public DateTime? OriginalStartDate { get; set; }
        public DateTime StartDate { get; set; }
        public virtual Event Event { get; set; }
        public virtual User User { get; set; }
        public bool IsRead { get; set; }
    }
}
