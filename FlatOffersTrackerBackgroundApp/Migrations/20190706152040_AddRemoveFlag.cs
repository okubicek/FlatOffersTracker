using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatOffersTrackerBackgroundApp.Migrations
{
    public partial class AddRemoveFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Removed",
                table: "FlatOffers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Removed",
                table: "FlatOffers");
        }
    }
}
