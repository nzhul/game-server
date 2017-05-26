namespace App.Data.Migrations
{
	using App.Models;
	using System.Data.Entity.Migrations;
	using System.Linq;

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
