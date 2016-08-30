namespace EventBot.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPlacesAndCostToEvent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "ParticipationCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Events", "MaxAttendees", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "MaxAttendees");
            DropColumn("dbo.Events", "ParticipationCost");
        }
    }
}
