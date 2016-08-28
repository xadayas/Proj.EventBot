namespace EventBot.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedStartDateToNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "StartDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "StartDate");
        }
    }
}
