namespace EventBot.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDbSetForNotifications : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.UserNotifications", new[] { "UserId" });
            DropPrimaryKey("dbo.UserNotifications");
            AlterColumn("dbo.UserNotifications", "Id", c => c.Int(nullable: false));
            AlterColumn("dbo.UserNotifications", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Notifications", "OriginalStartDate", c => c.DateTime());
            AddPrimaryKey("dbo.UserNotifications", new[] { "UserId", "NotificationId" });
            CreateIndex("dbo.UserNotifications", "UserId");
            DropColumn("dbo.Notifications", "OriginalEndDate");
            DropColumn("dbo.Notifications", "OriginalContent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notifications", "OriginalContent", c => c.String());
            AddColumn("dbo.Notifications", "OriginalEndDate", c => c.DateTime(nullable: false));
            DropIndex("dbo.UserNotifications", new[] { "UserId" });
            DropPrimaryKey("dbo.UserNotifications");
            AlterColumn("dbo.Notifications", "OriginalStartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.UserNotifications", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.UserNotifications", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.UserNotifications", "Id");
            CreateIndex("dbo.UserNotifications", "UserId");
        }
    }
}
