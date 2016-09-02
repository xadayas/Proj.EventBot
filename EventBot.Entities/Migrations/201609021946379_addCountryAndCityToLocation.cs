namespace EventBot.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCountryAndCityToLocation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "City", c => c.String());
            AddColumn("dbo.Locations", "Country", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Locations", "Country");
            DropColumn("dbo.Locations", "City");
        }
    }
}
