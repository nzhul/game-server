using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Models;
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

            modelBuilder.Entity<Realm>()
                .HasIndex(r => r.Name)
                .IsUnique(true);

            modelBuilder.Entity<Hero>()
                .HasOne(u => u.Avatar)
                .WithMany(u => u.Heroes)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Item>()
                .HasOne(u => u.Hero)
                .WithMany(u => u.Items)
                .OnDelete(DeleteBehavior.Cascade);
                
                
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            this.ApplyAudition();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            this.ApplyAudition();
            return base.SaveChanges();
        }

        private void ApplyAudition()
        {
            var entities = this.ChangeTracker.Entries()
                .Where(e => e.Entity is IAuditedEntity &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entities)
            {
                var entity = (IAuditedEntity)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }

                entity.ModifiedAt = DateTime.UtcNow;
            }
        }
    }
}
