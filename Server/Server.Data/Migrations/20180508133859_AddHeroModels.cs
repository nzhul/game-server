using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Server.Data.Migrations
{
    public partial class AddHeroModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HeroBlueprintClass",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AttackGainChance = table.Column<int>(nullable: false),
                    DefenseGainChance = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    MagicGainChance = table.Column<int>(nullable: false),
                    MagicPowerGainChance = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeroBlueprintClass", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HeroBlueprints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Attack = table.Column<int>(nullable: false),
                    Defense = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Dodge = table.Column<int>(nullable: false),
                    Health = table.Column<int>(nullable: false),
                    HeroClassId = table.Column<int>(nullable: false),
                    Magic = table.Column<int>(nullable: false),
                    MagicPower = table.Column<int>(nullable: false),
                    MagicResistance = table.Column<int>(nullable: false),
                    MaxDamage = table.Column<int>(nullable: false),
                    MinDamage = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PersonalAttack = table.Column<int>(nullable: false),
                    PersonalDefense = table.Column<int>(nullable: false),
                    PortraitImgUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeroBlueprints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HeroBlueprints_HeroBlueprintClass_HeroClassId",
                        column: x => x.HeroClassId,
                        principalTable: "HeroBlueprintClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Heroes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Attack = table.Column<int>(nullable: false),
                    BlueprintId = table.Column<int>(nullable: false),
                    Defence = table.Column<int>(nullable: false),
                    Dodge = table.Column<int>(nullable: false),
                    Health = table.Column<int>(nullable: false),
                    Magic = table.Column<int>(nullable: false),
                    MagicPower = table.Column<int>(nullable: false),
                    MagicResistance = table.Column<int>(nullable: false),
                    MaxDamage = table.Column<int>(nullable: false),
                    MinDamage = table.Column<int>(nullable: false),
                    PersonalAttack = table.Column<int>(nullable: false),
                    PersonalDefense = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Heroes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Heroes_HeroBlueprints_BlueprintId",
                        column: x => x.BlueprintId,
                        principalTable: "HeroBlueprints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HeroBlueprints_HeroClassId",
                table: "HeroBlueprints",
                column: "HeroClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Heroes_BlueprintId",
                table: "Heroes",
                column: "BlueprintId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Heroes");

            migrationBuilder.DropTable(
                name: "HeroBlueprints");

            migrationBuilder.DropTable(
                name: "HeroBlueprintClass");
        }
    }
}
