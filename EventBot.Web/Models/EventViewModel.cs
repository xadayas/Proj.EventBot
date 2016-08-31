using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace EventBot.Web.Models
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Title { get; set; }
        [Required]
        [MinLength(20)]
        [MaxLength(1000)]
        public string Description { get; set; }
        public int ImageId { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}",ApplyFormatInEditMode = true)]
        public Decimal ParticipationCost { get; set; }
        public int MaxAttendees { get; set; }
        public string Tags { get; set; }
        public LocationViewModel Location { get; set; }=new LocationViewModel();
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; } = DateTime.Now;
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}",ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; } =DateTime.Now;
        public bool IsCanceled { get; set; }
    }

    public class LocationViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Altitude { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    //public class EventTypeViewModel
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}