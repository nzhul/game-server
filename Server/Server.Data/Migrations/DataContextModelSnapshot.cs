﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server.Data;

namespace Server.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Server.Models.UnitConfigurations.Ability", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DamageAmount")
                        .HasColumnType("int");

                    b.Property<int>("HealingAmount")
                        .HasColumnType("int");

                    b.Property<bool>("IsHeroAbility")
                        .HasColumnType("bit");

                    b.Property<int>("Levels")
                        .HasColumnType("int");

                    b.Property<int>("Name")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Abilities");
                });

            modelBuilder.Entity("Server.Models.UnitConfigurations.UnitConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ActionPointsBase")
                        .HasColumnType("int");

                    b.Property<int>("ArmorBase")
                        .HasColumnType("int");

                    b.Property<int>("ArmorIncrement")
                        .HasColumnType("int");

                    b.Property<int>("ArmorType")
                        .HasColumnType("int");

                    b.Property<int>("AttackType")
                        .HasColumnType("int");

                    b.Property<int>("BuildTime")
                        .HasColumnType("int");

                    b.Property<int>("EvasionBase")
                        .HasColumnType("int");

                    b.Property<int>("Faction")
                        .HasColumnType("int");

                    b.Property<int>("FoodCost")
                        .HasColumnType("int");

                    b.Property<int>("GemsCost")
                        .HasColumnType("int");

                    b.Property<int>("GoldCost")
                        .HasColumnType("int");

                    b.Property<int>("HitpointsBase")
                        .HasColumnType("int");

                    b.Property<int>("HitpointsIncrement")
                        .HasColumnType("int");

                    b.Property<int>("ManaBase")
                        .HasColumnType("int");

                    b.Property<int>("ManaIncrement")
                        .HasColumnType("int");

                    b.Property<int>("MaxDamageBase")
                        .HasColumnType("int");

                    b.Property<int>("MaxDamageIncrement")
                        .HasColumnType("int");

                    b.Property<int>("MinDamageBase")
                        .HasColumnType("int");

                    b.Property<int>("MinDamageIncrement")
                        .HasColumnType("int");

                    b.Property<int>("MovementPointsBase")
                        .HasColumnType("int");

                    b.Property<int>("OreCost")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("WoodCost")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Type")
                        .IsUnique();

                    b.ToTable("UnitConfigurations");
                });

            modelBuilder.Entity("Server.Models.UnitConfigurations.UnitConfigurationAbility", b =>
                {
                    b.Property<int>("UnitConfigurationId")
                        .HasColumnType("int");

                    b.Property<int>("AbilityId")
                        .HasColumnType("int");

                    b.HasKey("UnitConfigurationId", "AbilityId");

                    b.HasIndex("AbilityId");

                    b.ToTable("UnitConfigurationAbility");
                });

            modelBuilder.Entity("Server.Models.UnitConfigurations.UnitConfigurationUpgrade", b =>
                {
                    b.Property<int>("UnitConfigurationId")
                        .HasColumnType("int");

                    b.Property<int>("UpgradeId")
                        .HasColumnType("int");

                    b.HasKey("UnitConfigurationId", "UpgradeId");

                    b.HasIndex("UpgradeId");

                    b.ToTable("UnitConfigurationUpgrade");
                });

            modelBuilder.Entity("Server.Models.UnitConfigurations.Upgrade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GoldCost")
                        .HasColumnType("int");

                    b.Property<int>("Name")
                        .HasColumnType("int");

                    b.Property<int>("TimeCost")
                        .HasColumnType("int");

                    b.Property<int>("WoodCost")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Upgrades");
                });

            modelBuilder.Entity("Server.Models.Items.ItemBlueprint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ItemSlotType")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ItemBlueprints");
                });

            modelBuilder.Entity("Server.Models.Games.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("WinnerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Server.Models.Users.Friendship", b =>
                {
                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.Property<int>("RecieverId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("BecameFriendsTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("RequestTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("SenderId", "RecieverId");

                    b.HasIndex("RecieverId");

                    b.ToTable("Friendships");
                });

            modelBuilder.Entity("Server.Models.Users.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateRead")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<DateTime>("MessageSent")
                        .HasColumnType("datetime2");

                    b.Property<bool>("RecipientDeleted")
                        .HasColumnType("bit");

                    b.Property<int?>("RecipientId")
                        .HasColumnType("int");

                    b.Property<bool>("SenderDeleted")
                        .HasColumnType("bit");

                    b.Property<int?>("SenderId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RecipientId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Server.Models.Users.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsMain")
                        .HasColumnType("bit");

                    b.Property<string>("PublicId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("Server.Models.Users.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Server.Models.Users.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<int>("ActiveConnection")
                        .HasColumnType("int");

                    b.Property<Guid?>("BattleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Discriminator")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<int?>("GameId")
                        .HasColumnType("int");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Interests")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastActive")
                        .HasColumnType("datetime2");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("MMR")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<byte>("OnlineStatus")
                        .HasColumnType("tinyint");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Server.Models.Users.UserRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Server.Models.Users.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("Server.Models.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("Server.Models.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("Server.Models.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Server.Models.UnitConfigurations.UnitConfigurationAbility", b =>
                {
                    b.HasOne("Server.Models.UnitConfigurations.Ability", "Ability")
                        .WithMany("UnitConfigurationAbilitys")
                        .HasForeignKey("AbilityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Models.UnitConfigurations.UnitConfiguration", "UnitConfiguration")
                        .WithMany("UnitConfigurationAbilitys")
                        .HasForeignKey("UnitConfigurationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ability");

                    b.Navigation("UnitConfiguration");
                });

            modelBuilder.Entity("Server.Models.UnitConfigurations.UnitConfigurationUpgrade", b =>
                {
                    b.HasOne("Server.Models.UnitConfigurations.UnitConfiguration", "UnitConfiguration")
                        .WithMany("UnitConfigurationUpgrades")
                        .HasForeignKey("UnitConfigurationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Models.UnitConfigurations.Upgrade", "Upgrade")
                        .WithMany("UnitConfigurationUpgrades")
                        .HasForeignKey("UpgradeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UnitConfiguration");

                    b.Navigation("Upgrade");
                });

            modelBuilder.Entity("Server.Models.Users.Friendship", b =>
                {
                    b.HasOne("Server.Models.Users.User", "Reciever")
                        .WithMany("RecievedFriendRequests")
                        .HasForeignKey("RecieverId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Server.Models.Users.User", "Sender")
                        .WithMany("SendFriendRequests")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Reciever");

                    b.Navigation("Sender");
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

                    b.Navigation("Recipient");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Server.Models.Users.Photo", b =>
                {
                    b.HasOne("Server.Models.Users.User", "User")
                        .WithMany("Photos")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Server.Models.Users.UserRole", b =>
                {
                    b.HasOne("Server.Models.Users.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Models.Users.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Server.Models.UnitConfigurations.Ability", b =>
                {
                    b.Navigation("UnitConfigurationAbilitys");
                });

            modelBuilder.Entity("Server.Models.UnitConfigurations.UnitConfiguration", b =>
                {
                    b.Navigation("UnitConfigurationAbilitys");

                    b.Navigation("UnitConfigurationUpgrades");
                });

            modelBuilder.Entity("Server.Models.UnitConfigurations.Upgrade", b =>
                {
                    b.Navigation("UnitConfigurationUpgrades");
                });

            modelBuilder.Entity("Server.Models.Users.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Server.Models.Users.User", b =>
                {
                    b.Navigation("MessagesRecieved");

                    b.Navigation("MessagesSent");

                    b.Navigation("Photos");

                    b.Navigation("RecievedFriendRequests");

                    b.Navigation("SendFriendRequests");

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
