namespace App.Data.Migrations
{
    using App.Models;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System;
    using Models.Towns;
    using Models.Towns.Human;

    public sealed class Configuration : DbMigrationsConfiguration<GameDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(GameDbContext context)
        {
            //this.AddInitialUsers(context);
            this.AddInitialTowns(context);
        }

        private void AddInitialTowns(GameDbContext context)
        {
            if (!context.Buildings.Any())
            {
                for (int i = 0; i < 6; i++)
                {
                    Building newBuilding = new Barracks
                    {
                        Name = "BARRACKS",
                        Level = 1,
                        BuildTimeInSeconds = 10,
                        Description = "This is the main building that produces ogres"
                    };
                    context.Buildings.Add(newBuilding);
                    context.SaveChanges();
                }
            }
        }

        private void AddInitialUsers(GameDbContext context)
        {
            if (!context.Users.Any())
            {
                for (int i = 1; i < 6; i++)
                {
                    var newUser = new AppUser
                    {
                        UserName = "User" + i
                    };
                    context.Users.Add(newUser);
                }
                context.SaveChanges();
            }
        }
    }
}
