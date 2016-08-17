using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBot.Entities.Service.Models
{
    // Dto model for Event, not complete
    // TODO Add Users, not really sure how to best implement it right now..
    public class EventModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string UserId { get; set; }
        //public User Organiser { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public long VisitCount { get; set; }
        public string MeatingPlace { get; set; }

        public int ImageId { get; set; }
        // public EventBotImage Image { get; set; }
        public bool IsCanceled { get; set; }
       
        public virtual ICollection<EventTypeModel> EventTypes { get; set; }
        //public virtual ICollection<EventUser> Users { get; set; }
    }
}
