using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatOffersTrackerBackgroundApp.Migrations
{
    public partial class AddingUniqueIdToLinks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UniqueId",
                table: "FlatOffersLinks",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqueId",
                table: "FlatOffersLinks");
        }
    }
}
