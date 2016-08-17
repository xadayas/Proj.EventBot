namespace EventBot.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Spellfix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "MeetingPlace", c => c.String());
            DropColumn("dbo.Events", "MeatingPlace");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "MeatingPlace", c => c.String());
            DropColumn("dbo.Events", "MeetingPlace");
        }
    }
}
