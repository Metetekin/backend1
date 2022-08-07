using Microsoft.EntityFrameworkCore.Migrations;

namespace BullBeez.Data.Migrations
{
    public partial class Version_17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InterestId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_InterestId",
                table: "Projects",
                column: "InterestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Interest_InterestId",
                table: "Projects",
                column: "InterestId",
                principalTable: "Interest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Interest_InterestId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_InterestId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "InterestId",
                table: "Projects");
        }
    }
}
