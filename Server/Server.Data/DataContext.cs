using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Models.Castles;
using Server.Models.Heroes;
using Server.Models.Heroes.Units;
using Server.Models.Items;
using Server.Models.MapEntities;
using Server.Models.Realms;
using Server.Models.Users;

namespace Server.Data
{
    public class DataContext : IdentityDbContext<User, Role, int,
        IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Avatar> Avatars { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Hero> Heroes { get; set; }

        public DbSet<HeroBlueprint> HeroBlueprints { get; set; }

        public DbSet<ItemBlueprint> ItemBlueprints { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Castle> Castles { get; set; }

        public DbSet<CastleBlueprint> CastleBlueprints { get; set; }

        public DbSet<Friendship> Friendships { get; set; }

        public DbSet<UnitConfiguration> UnitConfigurations { get; set; }

        public DbSet<Ability> Abilities { get; set; }

        public DbSet<Upgrade> Upgrades { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Friendship>()
                .HasKey(fs => new { fs.SenderId, fs.RecieverId });

            builder.Entity<Friendship>()
                .HasOne(u => u.Sender)
                .WithMany(fs => fs.SendFriendRequests)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(u => u.SenderId);

            builder.Entity<Friendship>()
                .HasOne(u => u.Reciever)
                .WithMany(fs => fs.RecievedFriendRequests)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(u => u.RecieverId);

            // Many to many
            //builder.Entity<AvatarDwelling>(avatarDwelling =>
            //{
            //    avatarDwelling.HasKey(ad => new { ad.AvatarId, ad.DwellingId });

            //    avatarDwelling.HasOne(ad => ad.Avatar)
            //    .WithMany(a => a.AvatarDwellings)
            //    .HasForeignKey(ad => ad.AvatarId)
            //    .IsRequired();

            //    avatarDwelling.HasOne(ad => ad.Dwelling)
            //    .WithMany(a => a.AvatarDwellings)
            //    .HasForeignKey(ad => ad.DwellingId)
            //    .IsRequired();
            //});

            builder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

                userRole.HasOne(ur => ur.User)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
            });

            builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(u => u.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(u => u.MessagesRecieved)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Hero>()
                .HasOne(u => u.Avatar)
                .WithMany(u => u.Heroes)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Hero>()
                .OwnsOne(x => x.NPCData);

            builder.Entity<Unit>()
                .HasOne(u => u.Hero)
                .WithMany(u => u.Units)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Item>()
                .HasOne(u => u.Hero)
                .WithMany(u => u.Items)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Game>()
                .Property(r => r.MatrixString)
                .HasField("_matrixString")
                .UsePropertyAccessMode(PropertyAccessMode.Property);

            builder.Entity<Room>()
                .Property(r => r.TilesString)
                .HasField("_tilesString")
                .UsePropertyAccessMode(PropertyAccessMode.Property);

            builder.Entity<Room>()
                .Property(r => r.EdgeTilesString)
                .HasField("_edgeTilesString")
                .UsePropertyAccessMode(PropertyAccessMode.Property);

            //builder.Entity<Room>()
            //    .Property(r => r.RoomSize)
            //    .HasField("_roomSize")
            //    .UsePropertyAccessMode(PropertyAccessMode.Property);

            builder.Entity<UnitConfiguration>()
                .HasIndex(x => x.Type)
                .IsUnique();

            // Many to many
            builder.Entity<UnitConfigurationAbility>(unitConfigurationAbility =>
            {
                unitConfigurationAbility.HasKey(ad => new { ad.UnitConfigurationId, ad.AbilityId });

                unitConfigurationAbility.HasOne(ad => ad.UnitConfiguration)
                .WithMany(a => a.UnitConfigurationAbilitys)
                .HasForeignKey(ad => ad.UnitConfigurationId)
                .IsRequired();

                unitConfigurationAbility.HasOne(ad => ad.Ability)
                .WithMany(a => a.UnitConfigurationAbilitys)
                .HasForeignKey(ad => ad.AbilityId)
                .IsRequired();
            });

            // Many to many
            builder.Entity<UnitConfigurationUpgrade>(unitConfigurationUpgrade =>
            {
                unitConfigurationUpgrade.HasKey(ad => new { ad.UnitConfigurationId, ad.UpgradeId });

                unitConfigurationUpgrade.HasOne(ad => ad.UnitConfiguration)
                .WithMany(a => a.UnitConfigurationUpgrades)
                .HasForeignKey(ad => ad.UnitConfigurationId)
                .IsRequired();

                unitConfigurationUpgrade.HasOne(ad => ad.Upgrade)
                .WithMany(a => a.UnitConfigurationUpgrades)
                .HasForeignKey(ad => ad.UpgradeId)
                .IsRequired();
            });

            builder.Entity<Dwelling>()
                .HasOne(u => u.Guardian);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            //this.ApplyAudition();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            //this.ApplyAudition();
            return base.SaveChanges();
        }

        //private void ApplyAudition()
        //{
        //    var entities = this.ChangeTracker.Entries()
        //        .Where(e => e.Entity is IAuditedEntity &&
        //            (e.State == EntityState.Added || e.State == EntityState.Modified));

        //    foreach (var entry in entities)
        //    {
        //        var entity = (IAuditedEntity)entry.Entity;

        //        if (entry.State == EntityState.Added)
        //        {
        //            entity.CreatedAt = DateTime.UtcNow;
        //        }

        //        entity.ModifiedAt = DateTime.UtcNow;
        //    }
        //}
    }
}
