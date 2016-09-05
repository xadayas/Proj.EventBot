namespace EventBot.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedThumbNailImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventBotImages", "ImageBytesLarge", c => c.Binary());
            AddColumn("dbo.EventBotImages", "ImageBytesThumb", c => c.Binary());
            DropColumn("dbo.EventBotImages", "ImageBytes");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EventBotImages", "ImageBytes", c => c.Binary());
            DropColumn("dbo.EventBotImages", "ImageBytesThumb");
            DropColumn("dbo.EventBotImages", "ImageBytesLarge");
        }
    }
}
