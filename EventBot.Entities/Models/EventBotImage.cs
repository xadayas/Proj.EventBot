namespace EventBot.Entities.Models
{
    public class EventBotImage
    {
        public int Id { get; set; }
        public byte[] ImageBytesLarge { get; set; }
        public byte[] ImageBytesThumb { get; set; }
    }
}