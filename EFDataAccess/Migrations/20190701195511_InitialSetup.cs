using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFRepository.Migrations
{
    public partial class InitialSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExecutionHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Success = table.Column<bool>(nullable: false),
                    DateTimeStarted = table.Column<DateTime>(nullable: false),
                    DateTimeFinished = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FlatOffers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: true),
                    FlatSize = table.Column<int>(nullable: false),
                    FlatType = table.Column<int>(nullable: false),
                    NumberOfRooms = table.Column<int>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateRemoved = table.Column<DateTime>(nullable: true),
                    Price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlatOffers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FlatOffersLinks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FlatOfferId = table.Column<int>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlatOffersLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlatOffersLinks_FlatOffers_FlatOfferId",
                        column: x => x.FlatOfferId,
                        principalTable: "FlatOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FlatOfferId = table.Column<int>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Viewed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_FlatOffers_FlatOfferId",
                        column: x => x.FlatOfferId,
                        principalTable: "FlatOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlatOffersLinks_FlatOfferId",
                table: "FlatOffersLinks",
                column: "FlatOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_FlatOfferId",
                table: "Notifications",
                column: "FlatOfferId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExecutionHistory");

            migrationBuilder.DropTable(
                name: "FlatOffersLinks");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "FlatOffers");
        }
    }
}
