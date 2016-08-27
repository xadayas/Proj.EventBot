namespace EventBot.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kopplingstabeller : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EventTypes", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.EventTypes", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "User_Id", "dbo.Users");
            DropIndex("dbo.EventTypes", new[] { "Event_Id" });
            DropIndex("dbo.EventTypes", new[] { "User_Id" });
            DropIndex("dbo.Users", new[] { "User_Id" });
            CreateTable(
                "dbo.EventEventType",
                c => new
                    {
                        EventId = c.Int(nullable: false),
                        EventTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EventId, t.EventTypeId })
                .ForeignKey("dbo.Events", t => t.EventId)
                .ForeignKey("dbo.EventTypes", t => t.EventTypeId)
                .Index(t => t.EventId)
                .Index(t => t.EventTypeId);
            
            CreateTable(
                "dbo.UserEventType",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        EventTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.EventTypeId })
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.EventTypes", t => t.EventTypeId)
                .Index(t => t.UserId)
                .Index(t => t.EventTypeId);
            
            CreateTable(
                "dbo.UserUser",
                c => new
                    {
                        FollowingId = c.String(nullable: false, maxLength: 128),
                        FollowewId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.FollowingId, t.FollowewId })
                .ForeignKey("dbo.Users", t => t.FollowingId)
                .ForeignKey("dbo.Users", t => t.FollowewId)
                .Index(t => t.FollowingId)
                .Index(t => t.FollowewId);
            
            DropColumn("dbo.EventTypes", "Event_Id");
            DropColumn("dbo.EventTypes", "User_Id");
            DropColumn("dbo.Users", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "User_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.EventTypes", "User_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.EventTypes", "Event_Id", c => c.Int());
            DropForeignKey("dbo.UserUser", "FollowewId", "dbo.Users");
            DropForeignKey("dbo.UserUser", "FollowingId", "dbo.Users");
            DropForeignKey("dbo.UserEventType", "EventTypeId", "dbo.EventTypes");
            DropForeignKey("dbo.UserEventType", "UserId", "dbo.Users");
            DropForeignKey("dbo.EventEventType", "EventTypeId", "dbo.EventTypes");
            DropForeignKey("dbo.EventEventType", "EventId", "dbo.Events");
            DropIndex("dbo.UserUser", new[] { "FollowewId" });
            DropIndex("dbo.UserUser", new[] { "FollowingId" });
            DropIndex("dbo.UserEventType", new[] { "EventTypeId" });
            DropIndex("dbo.UserEventType", new[] { "UserId" });
            DropIndex("dbo.EventEventType", new[] { "EventTypeId" });
            DropIndex("dbo.EventEventType", new[] { "EventId" });
            DropTable("dbo.UserUser");
            DropTable("dbo.UserEventType");
            DropTable("dbo.EventEventType");
            CreateIndex("dbo.Users", "User_Id");
            CreateIndex("dbo.EventTypes", "User_Id");
            CreateIndex("dbo.EventTypes", "Event_Id");
            AddForeignKey("dbo.Users", "User_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.EventTypes", "User_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.EventTypes", "Event_Id", "dbo.Events", "Id");
        }
    }
}
