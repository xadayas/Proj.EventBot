using System.Data.Entity;
using EventBot.Entities.Models;

namespace EventBot.Db
{
    public class EventBotDb : DbContext
    {
        // Your context has been configured to use a 'EventBotDb' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'EventBotEntities.EventBotDb' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'EventBotDb' 
        // connection string in the application configuration file.
        public EventBotDb()
            : base("name=EventBotDb")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<EventType> EventTypes { get; set; }
        public virtual DbSet<EventBotImage> Images { get; set; }
        public virtual DbSet<EventUser> EventUsers { get; set; }
    }


}