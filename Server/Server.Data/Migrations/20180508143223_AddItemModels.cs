using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Server.Data.Migrations
{
    public partial class AddItemModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Heroes_HeroBlueprints_BlueprintId",
                table: "Heroes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Heroes",
                table: "Heroes");

            migrationBuilder.RenameTable(
                name: "Heroes",
                newName: "Hero");

            migrationBuilder.RenameIndex(
                name: "IX_Heroes_BlueprintId",
                table: "Hero",
                newName: "IX_Hero_BlueprintId");

            migrationBuilder.AddColumn<int>(
                name: "WorldId",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "Hero",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Hero",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Hero",
                table: "Hero",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ItemBlueprints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    ItemSlotType = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemBlueprints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Worlds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Worlds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BlueprintId = table.Column<int>(nullable: false),
                    HeroId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_ItemBlueprints_BlueprintId",
                        column: x => x.BlueprintId,
                        principalTable: "ItemBlueprints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_Hero_HeroId",
                        column: x => x.HeroId,
                        principalTable: "Hero",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Level = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    WorldId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Regions_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_WorldId",
                table: "Users",
                column: "WorldId");

            migrationBuilder.CreateIndex(
                name: "IX_Hero_RegionId",
                table: "Hero",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Hero_UserId",
                table: "Hero",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_BlueprintId",
                table: "Items",
                column: "BlueprintId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_HeroId",
                table: "Items",
                column: "HeroId");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_WorldId",
                table: "Regions",
                column: "WorldId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hero_HeroBlueprints_BlueprintId",
                table: "Hero",
                column: "BlueprintId",
                principalTable: "HeroBlueprints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Hero_Regions_RegionId",
                table: "Hero",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Hero_Users_UserId",
                table: "Hero",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Worlds_WorldId",
                table: "Users",
                column: "WorldId",
                principalTable: "Worlds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hero_HeroBlueprints_BlueprintId",
                table: "Hero");

            migrationBuilder.DropForeignKey(
                name: "FK_Hero_Regions_RegionId",
                table: "Hero");

            migrationBuilder.DropForeignKey(
                name: "FK_Hero_Users_UserId",
                table: "Hero");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Worlds_WorldId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "ItemBlueprints");

            migrationBuilder.DropTable(
                name: "Worlds");

            migrationBuilder.DropIndex(
                name: "IX_Users_WorldId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Hero",
                table: "Hero");

            migrationBuilder.DropIndex(
                name: "IX_Hero_RegionId",
                table: "Hero");

            migrationBuilder.DropIndex(
                name: "IX_Hero_UserId",
                table: "Hero");

            migrationBuilder.DropColumn(
                name: "WorldId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Hero");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Hero");

            migrationBuilder.RenameTable(
                name: "Hero",
                newName: "Heroes");

            migrationBuilder.RenameIndex(
                name: "IX_Hero_BlueprintId",
                table: "Heroes",
                newName: "IX_Heroes_BlueprintId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Heroes",
                table: "Heroes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Heroes_HeroBlueprints_BlueprintId",
                table: "Heroes",
                column: "BlueprintId",
                principalTable: "HeroBlueprints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
