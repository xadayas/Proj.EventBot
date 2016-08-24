namespace EventBot.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedOrganisedEventsToUserEntity : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Events", new[] { "Organiser_Id" });
            AlterColumn("dbo.Events", "Organiser_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Events", "Organiser_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Events", new[] { "Organiser_Id" });
            AlterColumn("dbo.Events", "Organiser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Events", "Organiser_Id");
        }
    }
}
