﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server.Data;

namespace Server.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20181021141800_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<int>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Server.Models.Castles.Castle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AvatarId");

                    b.Property<int?>("BlueprintId");

                    b.Property<int?>("RegionId");

                    b.HasKey("Id");

                    b.HasIndex("AvatarId");

                    b.HasIndex("BlueprintId");

                    b.HasIndex("RegionId");

                    b.ToTable("Castles");
                });

            modelBuilder.Entity("Server.Models.Castles.CastleBlueprint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("CastleBlueprints");
                });

            modelBuilder.Entity("Server.Models.Heroes.Hero", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Attack");

                    b.Property<int?>("AvatarId");

                    b.Property<int?>("BlueprintId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<int>("Defence");

                    b.Property<int>("Dodge");

                    b.Property<int>("Health");

                    b.Property<DateTime>("LastActivity");

                    b.Property<int>("Level");

                    b.Property<int>("Magic");

                    b.Property<int>("MagicPower");

                    b.Property<int>("MagicResistance");

                    b.Property<int>("MaxDamage");

                    b.Property<int>("MinDamage");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name");

                    b.Property<int>("PersonalAttack");

                    b.Property<int>("PersonalDefense");

                    b.Property<int?>("RegionId");

                    b.Property<long>("TimePlayedTicks");

                    b.Property<int>("X");

                    b.Property<int>("Y");

                    b.HasKey("Id");

                    b.HasIndex("AvatarId");

                    b.HasIndex("BlueprintId");

                    b.HasIndex("RegionId");

                    b.ToTable("Heroes");
                });

            modelBuilder.Entity("Server.Models.Heroes.HeroBlueprint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Attack");

                    b.Property<int>("Class");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<int>("Defense");

                    b.Property<string>("Description");

                    b.Property<int>("Dodge");

                    b.Property<int>("Faction");

                    b.Property<int>("Health");

                    b.Property<int>("Magic");

                    b.Property<int>("MagicPower");

                    b.Property<int>("MagicResistance");

                    b.Property<int>("MaxDamage");

                    b.Property<int>("MinDamage");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("ModifiedBy");

                    b.Property<int>("PersonalAttack");

                    b.Property<int>("PersonalDefense");

                    b.Property<string>("PortraitImgUrl");

                    b.HasKey("Id");

                    b.ToTable("HeroBlueprints");
                });

            modelBuilder.Entity("Server.Models.Items.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BlueprintId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<int?>("HeroId");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("ModifiedBy");

                    b.HasKey("Id");

                    b.HasIndex("BlueprintId");

                    b.HasIndex("HeroId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Server.Models.Items.ItemBlueprint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("Description");

                    b.Property<int>("ItemSlotType");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("ItemBlueprints");
                });

            modelBuilder.Entity("Server.Models.MapEntities.Dwelling", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name");

                    b.Property<string>("OccupiedTilesString");

                    b.Property<int>("OwnerId");

                    b.Property<int?>("RegionId");

                    b.Property<int>("Type");

                    b.Property<int>("X");

                    b.Property<int>("Y");

                    b.HasKey("Id");

                    b.HasIndex("RegionId");

                    b.ToTable("Dwelling");
                });

            modelBuilder.Entity("Server.Models.MapEntities.MonsterPack", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<int>("Disposition");

                    b.Property<int?>("ItemRewardId");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name");

                    b.Property<string>("OccupiedTilesString");

                    b.Property<int>("Quantity");

                    b.Property<int?>("RegionId");

                    b.Property<int>("RewardQuantity");

                    b.Property<int>("RewardType");

                    b.Property<int>("TroopsRewardQuantity");

                    b.Property<int>("TroopsRewardType");

                    b.Property<int>("Type");

                    b.Property<int>("X");

                    b.Property<int>("Y");

                    b.HasKey("Id");

                    b.HasIndex("ItemRewardId");

                    b.HasIndex("RegionId");

                    b.ToTable("MonsterPack");
                });

            modelBuilder.Entity("Server.Models.MapEntities.Treasure", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name");

                    b.Property<int>("Quantity");

                    b.Property<int?>("RegionId");

                    b.Property<int>("Type");

                    b.Property<int>("X");

                    b.Property<int>("Y");

                    b.HasKey("Id");

                    b.HasIndex("RegionId");

                    b.ToTable("Treasure");
                });

            modelBuilder.Entity("Server.Models.Realms.Realm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name");

                    b.Property<DateTime>("ResetDate");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("Realms");
                });

            modelBuilder.Entity("Server.Models.Realms.Region", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<int>("Level");

                    b.Property<string>("MatrixString");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("Name");

                    b.Property<int?>("RealmId");

                    b.HasKey("Id");

                    b.HasIndex("RealmId");

                    b.ToTable("Regions");
                });

            modelBuilder.Entity("Server.Models.Realms.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("EdgeTilesString");

                    b.Property<bool>("IsAccessibleFromMainRoom");

                    b.Property<bool>("IsMainRoom");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("ModifiedBy");

                    b.Property<int?>("RegionId");

                    b.Property<int>("RoomSize");

                    b.Property<string>("TilesString");

                    b.HasKey("Id");

                    b.HasIndex("RegionId");

                    b.ToTable("Room");
                });

            modelBuilder.Entity("Server.Models.Users.Avatar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<int>("Gems");

                    b.Property<int>("Gold");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("ModifiedBy");

                    b.Property<int>("Ore");

                    b.Property<int?>("RealmId");

                    b.Property<int?>("UserId");

                    b.Property<int>("Wood");

                    b.HasKey("Id");

                    b.HasIndex("RealmId");

                    b.HasIndex("UserId");

                    b.ToTable("Avatars");
                });

            modelBuilder.Entity("Server.Models.Users.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("DateRead");

                    b.Property<bool>("IsRead");

                    b.Property<DateTime>("MessageSent");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("ModifiedBy");

                    b.Property<bool>("RecipientDeleted");

                    b.Property<int?>("RecipientId");

                    b.Property<bool>("SenderDeleted");

                    b.Property<int?>("SenderId");

                    b.HasKey("Id");

                    b.HasIndex("RecipientId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Server.Models.Users.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateAdded");

                    b.Property<string>("Description");

                    b.Property<bool>("IsMain");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("PublicId");

                    b.Property<string>("Url");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("Server.Models.Users.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Server.Models.Users.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("City");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Country");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<int>("CurrentRealmId");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("Gender");

                    b.Property<string>("Interests");

                    b.Property<DateTime>("LastActive");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Server.Models.Users.UserRole", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Server.Models.Users.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("Server.Models.Users.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("Server.Models.Users.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("Server.Models.Users.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Server.Models.Castles.Castle", b =>
                {
                    b.HasOne("Server.Models.Users.Avatar")
                        .WithMany("Castles")
                        .HasForeignKey("AvatarId");

                    b.HasOne("Server.Models.Castles.CastleBlueprint", "Blueprint")
                        .WithMany()
                        .HasForeignKey("BlueprintId");

                    b.HasOne("Server.Models.Realms.Region", "Region")
                        .WithMany("Castles")
                        .HasForeignKey("RegionId");
                });

            modelBuilder.Entity("Server.Models.Heroes.Hero", b =>
                {
                    b.HasOne("Server.Models.Users.Avatar", "Avatar")
                        .WithMany("Heroes")
                        .HasForeignKey("AvatarId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Server.Models.Heroes.HeroBlueprint", "Blueprint")
                        .WithMany()
                        .HasForeignKey("BlueprintId");

                    b.HasOne("Server.Models.Realms.Region", "Region")
                        .WithMany("Heroes")
                        .HasForeignKey("RegionId");
                });

            modelBuilder.Entity("Server.Models.Items.Item", b =>
                {
                    b.HasOne("Server.Models.Items.ItemBlueprint", "Blueprint")
                        .WithMany()
                        .HasForeignKey("BlueprintId");

                    b.HasOne("Server.Models.Heroes.Hero", "Hero")
                        .WithMany("Items")
                        .HasForeignKey("HeroId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Server.Models.MapEntities.Dwelling", b =>
                {
                    b.HasOne("Server.Models.Realms.Region")
                        .WithMany("Dwellings")
                        .HasForeignKey("RegionId");
                });

            modelBuilder.Entity("Server.Models.MapEntities.MonsterPack", b =>
                {
                    b.HasOne("Server.Models.Items.ItemBlueprint", "ItemReward")
                        .WithMany()
                        .HasForeignKey("ItemRewardId");

                    b.HasOne("Server.Models.Realms.Region")
                        .WithMany("MonsterPacks")
                        .HasForeignKey("RegionId");
                });

            modelBuilder.Entity("Server.Models.MapEntities.Treasure", b =>
                {
                    b.HasOne("Server.Models.Realms.Region")
                        .WithMany("Treasures")
                        .HasForeignKey("RegionId");
                });

            modelBuilder.Entity("Server.Models.Realms.Region", b =>
                {
                    b.HasOne("Server.Models.Realms.Realm", "Realm")
                        .WithMany("Regions")
                        .HasForeignKey("RealmId");
                });

            modelBuilder.Entity("Server.Models.Realms.Room", b =>
                {
                    b.HasOne("Server.Models.Realms.Region")
                        .WithMany("Rooms")
                        .HasForeignKey("RegionId");
                });

            modelBuilder.Entity("Server.Models.Users.Avatar", b =>
                {
                    b.HasOne("Server.Models.Realms.Realm", "Realm")
                        .WithMany("Avatars")
                        .HasForeignKey("RealmId");

                    b.HasOne("Server.Models.Users.User", "User")
                        .WithMany("Avatars")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Server.Models.Users.Message", b =>
                {
                    b.HasOne("Server.Models.Users.User", "Recipient")
                        .WithMany("MessagesRecieved")
                        .HasForeignKey("RecipientId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Server.Models.Users.User", "Sender")
                        .WithMany("MessagesSent")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Server.Models.Users.Photo", b =>
                {
                    b.HasOne("Server.Models.Users.User", "User")
                        .WithMany("Photos")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Server.Models.Users.UserRole", b =>
                {
                    b.HasOne("Server.Models.Users.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Server.Models.Users.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}