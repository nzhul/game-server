using Microsoft.EntityFrameworkCore;
using Server.Models.Blueprints;
using Server.Models.Heroes;
using Server.Models.Users;

namespace Server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        #region Users

        public DbSet<User> Users { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<HeroBlueprint> HeroBlueprints { get; set; }

        public DbSet<HeroBlueprintClass> HeroBlueprintClass { get; set; }

        public DbSet<Hero> Heroes { get; set; }


        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(u => u.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(u => u.MessagesRecieved)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
