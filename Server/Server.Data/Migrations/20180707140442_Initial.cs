using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CastleBlueprints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CastleBlueprints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HeroBlueprints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    PortraitImgUrl = table.Column<string>(nullable: true),
                    Faction = table.Column<int>(nullable: false),
                    Class = table.Column<int>(nullable: false),
                    Attack = table.Column<int>(nullable: false),
                    Defense = table.Column<int>(nullable: false),
                    PersonalAttack = table.Column<int>(nullable: false),
                    PersonalDefense = table.Column<int>(nullable: false),
                    Magic = table.Column<int>(nullable: false),
                    MagicPower = table.Column<int>(nullable: false),
                    Dodge = table.Column<int>(nullable: false),
                    Health = table.Column<int>(nullable: false),
                    MinDamage = table.Column<int>(nullable: false),
                    MaxDamage = table.Column<int>(nullable: false),
                    MagicResistance = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeroBlueprints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemBlueprints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ItemSlotType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemBlueprints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Realms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ResetDate = table.Column<DateTime>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Realms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSald = table.Column<byte[]>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    LastActive = table.Column<DateTime>(nullable: false),
                    Interests = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    CurrentRealmId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Level = table.Column<int>(nullable: false),
                    RealmId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Regions_Realms_RealmId",
                        column: x => x.RealmId,
                        principalTable: "Realms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Avatars",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    Wood = table.Column<int>(nullable: false),
                    Ore = table.Column<int>(nullable: false),
                    Gold = table.Column<int>(nullable: false),
                    Gems = table.Column<int>(nullable: false),
                    RealmId = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avatars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Avatars_Realms_RealmId",
                        column: x => x.RealmId,
                        principalTable: "Realms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Avatars_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    SenderId = table.Column<int>(nullable: true),
                    RecipientId = table.Column<int>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    IsRead = table.Column<bool>(nullable: false),
                    DateRead = table.Column<DateTime>(nullable: true),
                    MessageSent = table.Column<DateTime>(nullable: false),
                    SenderDeleted = table.Column<bool>(nullable: false),
                    RecipientDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Users_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    IsMain = table.Column<bool>(nullable: false),
                    PublicId = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Castles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BlueprintId = table.Column<int>(nullable: true),
                    RegionId = table.Column<int>(nullable: true),
                    AvatarId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Castles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Castles_Avatars_AvatarId",
                        column: x => x.AvatarId,
                        principalTable: "Avatars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Castles_CastleBlueprints_BlueprintId",
                        column: x => x.BlueprintId,
                        principalTable: "CastleBlueprints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Castles_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Hero",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LastActivity = table.Column<DateTime>(nullable: false),
                    TimePlayedTicks = table.Column<long>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    Attack = table.Column<int>(nullable: false),
                    Defence = table.Column<int>(nullable: false),
                    Magic = table.Column<int>(nullable: false),
                    MagicPower = table.Column<int>(nullable: false),
                    PersonalAttack = table.Column<int>(nullable: false),
                    PersonalDefense = table.Column<int>(nullable: false),
                    Dodge = table.Column<int>(nullable: false),
                    Health = table.Column<int>(nullable: false),
                    MinDamage = table.Column<int>(nullable: false),
                    MaxDamage = table.Column<int>(nullable: false),
                    MagicResistance = table.Column<int>(nullable: false),
                    BlueprintId = table.Column<int>(nullable: true),
                    RegionId = table.Column<int>(nullable: true),
                    AvatarId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hero", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hero_Avatars_AvatarId",
                        column: x => x.AvatarId,
                        principalTable: "Avatars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Hero_HeroBlueprints_BlueprintId",
                        column: x => x.BlueprintId,
                        principalTable: "HeroBlueprints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Hero_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    BlueprintId = table.Column<int>(nullable: true),
                    HeroId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_ItemBlueprints_BlueprintId",
                        column: x => x.BlueprintId,
                        principalTable: "ItemBlueprints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Items_Hero_HeroId",
                        column: x => x.HeroId,
                        principalTable: "Hero",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Avatars_RealmId",
                table: "Avatars",
                column: "RealmId");

            migrationBuilder.CreateIndex(
                name: "IX_Avatars_UserId",
                table: "Avatars",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Castles_AvatarId",
                table: "Castles",
                column: "AvatarId");

            migrationBuilder.CreateIndex(
                name: "IX_Castles_BlueprintId",
                table: "Castles",
                column: "BlueprintId");

            migrationBuilder.CreateIndex(
                name: "IX_Castles_RegionId",
                table: "Castles",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Hero_AvatarId",
                table: "Hero",
                column: "AvatarId");

            migrationBuilder.CreateIndex(
                name: "IX_Hero_BlueprintId",
                table: "Hero",
                column: "BlueprintId");

            migrationBuilder.CreateIndex(
                name: "IX_Hero_RegionId",
                table: "Hero",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_BlueprintId",
                table: "Items",
                column: "BlueprintId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_HeroId",
                table: "Items",
                column: "HeroId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RecipientId",
                table: "Messages",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_UserId",
                table: "Photos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Realms_Name",
                table: "Realms",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_RealmId",
                table: "Regions",
                column: "RealmId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Castles");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "CastleBlueprints");

            migrationBuilder.DropTable(
                name: "ItemBlueprints");

            migrationBuilder.DropTable(
                name: "Hero");

            migrationBuilder.DropTable(
                name: "Avatars");

            migrationBuilder.DropTable(
                name: "HeroBlueprints");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Realms");
        }
    }
}
