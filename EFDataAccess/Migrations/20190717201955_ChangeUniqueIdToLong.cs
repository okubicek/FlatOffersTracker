using Microsoft.EntityFrameworkCore.Migrations;

namespace EFRepository.Migrations
{
    public partial class ChangeUniqueIdToLong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "UniqueId",
                table: "FlatOffersLinks",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UniqueId",
                table: "FlatOffersLinks",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
