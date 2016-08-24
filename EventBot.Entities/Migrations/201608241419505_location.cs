namespace EventBot.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class location : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Altitude = c.Double(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Events", "Location_Id", c => c.Int());
            CreateIndex("dbo.Events", "Location_Id");
            AddForeignKey("dbo.Events", "Location_Id", "dbo.Locations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "Location_Id", "dbo.Locations");
            DropIndex("dbo.Events", new[] { "Location_Id" });
            DropColumn("dbo.Events", "Location_Id");
            DropTable("dbo.Locations");
        }
    }
}
