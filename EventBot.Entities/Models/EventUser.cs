namespace EventBot.Entities.Models
{
    public class EventUser
    {
        public int Id { get; set; }
        public Event Event { get; set; }
        public User User { get; set; }
        public float Rating { get; set; }
    }
}
