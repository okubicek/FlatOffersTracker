using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFRepository.Migrations
{
    public partial class AddingImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlatOfferImages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FlatOfferId = table.Column<int>(nullable: true),
                    Content = table.Column<byte[]>(nullable: true),
                    SortOrder = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlatOfferImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlatOfferImages_FlatOffers_FlatOfferId",
                        column: x => x.FlatOfferId,
                        principalTable: "FlatOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlatOfferImages_FlatOfferId",
                table: "FlatOfferImages",
                column: "FlatOfferId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlatOfferImages");
        }
    }
}