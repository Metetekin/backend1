using Microsoft.EntityFrameworkCore.Migrations;

namespace BullBeez.Data.Migrations
{
    public partial class Version_111 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyAndPersonOccupation_Occupations_OccupationId",
                table: "CompanyAndPersonOccupation");

            migrationBuilder.AlterColumn<int>(
                name: "OccupationId",
                table: "CompanyAndPersonOccupation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ComponyAndPersonId",
                table: "CompanyAndPersonOccupation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyAndPersonOccupation_Occupations_OccupationId",
                table: "CompanyAndPersonOccupation",
                column: "OccupationId",
                principalTable: "Occupations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyAndPersonOccupation_Occupations_OccupationId",
                table: "CompanyAndPersonOccupation");

            migrationBuilder.DropColumn(
                name: "ComponyAndPersonId",
                table: "CompanyAndPersonOccupation");

            migrationBuilder.AlterColumn<int>(
                name: "OccupationId",
                table: "CompanyAndPersonOccupation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyAndPersonOccupation_Occupations_OccupationId",
                table: "CompanyAndPersonOccupation",
                column: "OccupationId",
                principalTable: "Occupations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
