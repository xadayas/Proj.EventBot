using System.Collections.Generic;
using System.Text;
using EventBot.Entities.Models;
using Microsoft.AspNet.Identity;

namespace EventBot.Entities.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EventBot.Entities.EventBotDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EventBot.Entities.EventBotDb context)
        {
            
            var rnd = new Random();
            // 5 test users
            string[] testIds = {"random1", "random2", "random3", "random4", "random5"};
            string[] testNames = {"Johan Jonsson", "Eva Andersson", "Erik Mjunstedt", "Agneta Hård", "Ivar Von Hu"};
            string[] testUserNames = {"johan@eventbot.se", "eva@eventbot.se", "erik@eventbot.se", "agneta@eventbot.se", "ivar@eventbot.se"};
            const string testPassword = "Passw0rd!";

            //EventTypes
            string[] testEventTypes = {"SvampPlockning", "Pokemon", "Bergsklättring", "Segling", "Cykling"};

            // test events
            string[] testEventTitles =
            {
                "Plocka Svamp",
                "Jaga pokemon",
                "Klättra",
                "Segla",
                "Cykla"
            };
            string[] testEventDescriptions =
            {
                "asdasdasdasdasd",
                "asdasdasdasdasdasd",
                "asdaskdsaldkfjakzsdjfhklas",
                "sladkfaksfnksdfvosidjfpaow",
                "asldkfjaobfilxkjvkwajefoawjrel"
            };
            string[] testPlaces =
            {
                "Umeå",
                "Örnsköldsvik",
                "Kramfors",
                "Rundvik",
                "Stockolm"
            };

            //Image image
            context.Images.AddOrUpdate(x => x.Id, new EventBotImage
            {
                Id = 1,
                ImageBytes = Encoding.UTF8.GetBytes("0x89504E470D0A1A0A0000000D4948445200000075000000560806000000F13CD924000000206348524D00007A26000080840000FA00000080E8000075300000EA6000003A98000017709CBA513C000000017352474200AECE1CE90000000467414D410000B18F0BFC6105000000097048597300000B1300000B1301009A9C1800")
            });

            for (var i = 0; i < 5; i++)
            {
                context.EventTypes.AddOrUpdate(x=>x.Id,
                    new EventType
                    {
                        Id = i+1,
                        Name = testEventTypes[i]
                    });
            }


            var hasher = new PasswordHasher();
            var hashedPassword = hasher.HashPassword(testPassword);
            for (var i = 0; i < 5; i++)
            {
                context.Users.AddOrUpdate(x => x.Id,
                    new User
                    {
                        Id = testIds[i],
                        SecurityStamp = testIds[i],
                        Name = testNames[i],
                        UserName = testUserNames[i],
                        Email = testUserNames[i],
                        PasswordHash = hashedPassword,
                        Image = context.Images.Find(1)
                    });
            }




            for (var i = 0; i < 5; i++)
            {
                context.Events.AddOrUpdate(x => x.Id,
                    new Event
                    {
                        Id = i + 1,
                        Organiser = context.Users.Find(testIds[i]),
                        Title = testEventTitles[i],
                        Description = testEventDescriptions[i],
                        MeetingPlace = testPlaces[i],
                        CreatedDate = DateTime.Now,
                        StartDate = DateTime.Now.AddHours(1),
                        EndDate = DateTime.Now.AddHours(2),
                        ModifiedDate = DateTime.Now,
                        VisitCount = rnd.Next(0,1000),
                        Image = context.Images.Find(1)
                    });
            }

            for (var i = 0; i < 5; i++)
            {
                context.EventUsers.AddOrUpdate(x=>x.Id,
                    new EventUser[]
                    {
                        new EventUser {Id=i*5+1,Event = context.Events.Find(i+1),Rating = rnd.Next(0,10),User = context.Users.Find(testIds[0])},
                        new EventUser {Id=i*5+2,Event = context.Events.Find(i+1),Rating = rnd.Next(0,10),User = context.Users.Find(testIds[1])},
                        new EventUser {Id=i*5+3,Event = context.Events.Find(i+1),Rating = rnd.Next(0,10),User = context.Users.Find(testIds[2])},
                        new EventUser {Id=i*5+4,Event = context.Events.Find(i+1),Rating = rnd.Next(0,10),User = context.Users.Find(testIds[3])},
                        new EventUser {Id=i*5+5,Event = context.Events.Find(i+1),Rating = rnd.Next(0,10),User = context.Users.Find(testIds[4])},
                    });
            }

           
            
            
            context.SaveChanges();
        }
    }
}
