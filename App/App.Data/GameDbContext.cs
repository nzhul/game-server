using App.Data.Migrations;
using App.Models;
using App.Models.Heroes;
using App.Models.Towns;
using App.Models.Units;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace App.Data
{
	public class GameDbContext : IdentityDbContext<AppUser>
    {
        public GameDbContext()
            : base("App")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<GameDbContext, Configuration>());
        }

        public static GameDbContext Create()
        {
            return new GameDbContext();
        }

        //public IDbSet<AppUser> Users { get; set; }
        public IDbSet<Unit> Units { get; set; }
        public IDbSet<Hero> Heroes { get; set; }
        public IDbSet<Town> Towns { get; set; }
        public IDbSet<Building> Buildings { get; set; }
    }
}
