using Microsoft.EntityFrameworkCore;
using Server.Models.Castles;
using Server.Models.Heroes;
using Server.Models.Items;
using Server.Models.Realms;
using Server.Models.Users;

namespace Server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Avatar> Avatars { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<HeroBlueprint> HeroBlueprints { get; set; }

        public DbSet<HeroBlueprintClass> HeroBlueprintClass { get; set; }

        public DbSet<ItemBlueprint> ItemBlueprints { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Realm> Realms { get; set; }

        public DbSet<Region> Regions { get; set; }

        public DbSet<Castle> Castles { get; set; }

        public DbSet<CastleBlueprint> CastleBlueprints { get; set; }

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
