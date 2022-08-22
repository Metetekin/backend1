using Microsoft.EntityFrameworkCore.Migrations;

namespace BullBeez.Data.Migrations
{
    public partial class Version_136 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Color",
                table: "Packages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProfileType",
                table: "Packages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "ProfileType",
                table: "Packages");
        }
    }
}
