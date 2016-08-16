namespace EventBot.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        VisitCount = c.Long(nullable: false),
                        MeatingPlace = c.String(),
                        IsCanceled = c.Boolean(nullable: false),
                        Image_Id = c.Int(),
                        Organiser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EventBotImages", t => t.Image_Id)
                .ForeignKey("dbo.Users", t => t.Organiser_Id)
                .Index(t => t.Image_Id)
                .Index(t => t.Organiser_Id);
            
            CreateTable(
                "dbo.EventTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Event_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.Event_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Event_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.EventBotImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImageBytes = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsCompany = c.Boolean(nullable: false),
                        User_Id = c.Int(),
                        Image_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .ForeignKey("dbo.EventBotImages", t => t.Image_Id)
                .Index(t => t.User_Id)
                .Index(t => t.Image_Id);
            
            CreateTable(
                "dbo.EventUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rating = c.Single(nullable: false),
                        Event_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.Event_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Event_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "Organiser_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "Image_Id", "dbo.EventBotImages");
            DropForeignKey("dbo.Users", "User_Id", "dbo.Users");
            DropForeignKey("dbo.EventTypes", "User_Id", "dbo.Users");
            DropForeignKey("dbo.EventUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.EventUsers", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.Events", "Image_Id", "dbo.EventBotImages");
            DropForeignKey("dbo.EventTypes", "Event_Id", "dbo.Events");
            DropIndex("dbo.EventUsers", new[] { "User_Id" });
            DropIndex("dbo.EventUsers", new[] { "Event_Id" });
            DropIndex("dbo.Users", new[] { "Image_Id" });
            DropIndex("dbo.Users", new[] { "User_Id" });
            DropIndex("dbo.EventTypes", new[] { "User_Id" });
            DropIndex("dbo.EventTypes", new[] { "Event_Id" });
            DropIndex("dbo.Events", new[] { "Organiser_Id" });
            DropIndex("dbo.Events", new[] { "Image_Id" });
            DropTable("dbo.EventUsers");
            DropTable("dbo.Users");
            DropTable("dbo.EventBotImages");
            DropTable("dbo.EventTypes");
            DropTable("dbo.Events");
        }
    }
}
