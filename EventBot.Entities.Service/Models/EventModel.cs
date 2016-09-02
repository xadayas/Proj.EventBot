using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [DisplayFormat(DataFormatString = "{0:dddd dd MMMM yyyy}")]
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public long VisitCount { get; set; }
        public string MeetingPlace { get; set; }
        public Decimal ParticipationCost { get; set; }
        public int MaxAttendees { get; set; }
        public LocationModel Location { get; set; }
        public double DistanceFromClient { get; set;}

        public int ImageId { get; set; }
        public bool IsCanceled { get; set; }
        public virtual ICollection<EventTypeModel> EventTypes { get; set; }
        public int UserCount { get; set; } 
    }
}
