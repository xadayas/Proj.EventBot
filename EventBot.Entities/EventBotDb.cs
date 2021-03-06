using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using EventBot.Entities.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EventBot.Entities
{
    public class EventBotDb : IdentityDbContext<User>
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
        public static EventBotDb Create()
        {
            return new EventBotDb();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .ToTable("Users");
            modelBuilder.Entity<IdentityRole>()
                .ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole>()
                .ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim>()
                .ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin>()
                .ToTable("UserLogins");
            modelBuilder.Entity<User>().HasMany(p => p.AttendingEvents).WithMany(p=>p.Users).Map(x =>
            {
                x.MapLeftKey("UserId");
                x.MapRightKey("EventId");
                x.ToTable("EventUser");
            });
            modelBuilder.Entity<Event>().HasMany(p => p.EventTypes).WithMany().Map(x =>
            {
                x.MapLeftKey("EventId");
                x.MapRightKey("EventTypeId");
                x.ToTable("EventEventType");
            });
            modelBuilder.Entity<User>().HasMany(p => p.EventTypeInterests).WithMany().Map(x =>
            {
                x.MapLeftKey("UserId");
                x.MapRightKey("EventTypeId");
                x.ToTable("UserEventType");
            });
            modelBuilder.Entity<User>().HasMany(p => p.FollowingUsers).WithMany(p => p.Followers).Map(x =>
            {
                x.MapLeftKey("FollowingId");
                x.MapRightKey("FollowewId");
                x.ToTable("UserUser");
            });
        }
        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<EventType> EventTypes { get; set; }
        public virtual DbSet<EventBotImage> Images { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Location> Locations { get; set; } 
    }
}