using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BullBeez.Data.Migrations
{
    public partial class Version_112 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVenture",
                table: "CompanyType",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CompanyLevelId",
                table: "CompanyAndPerson",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EstablishDate",
                table: "CompanyAndPerson",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CompanyLevel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    RowStatu = table.Column<int>(type: "int", nullable: true),
                    InsertedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InsertedIp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InsertedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedIp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyLevel", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAndPerson_CompanyLevelId",
                table: "CompanyAndPerson",
                column: "CompanyLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyAndPerson_CompanyLevel_CompanyLevelId",
                table: "CompanyAndPerson",
                column: "CompanyLevelId",
                principalTable: "CompanyLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyAndPerson_CompanyLevel_CompanyLevelId",
                table: "CompanyAndPerson");

            migrationBuilder.DropTable(
                name: "CompanyLevel");

            migrationBuilder.DropIndex(
                name: "IX_CompanyAndPerson_CompanyLevelId",
                table: "CompanyAndPerson");

            migrationBuilder.DropColumn(
                name: "IsVenture",
                table: "CompanyType");

            migrationBuilder.DropColumn(
                name: "CompanyLevelId",
                table: "CompanyAndPerson");

            migrationBuilder.DropColumn(
                name: "EstablishDate",
                table: "CompanyAndPerson");
        }
    }
}
