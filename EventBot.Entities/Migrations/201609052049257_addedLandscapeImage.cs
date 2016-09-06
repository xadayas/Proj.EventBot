namespace EventBot.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedLandscapeImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventBotImages", "ImageBytesLandscape", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventBotImages", "ImageBytesLandscape");
        }
    }
}
