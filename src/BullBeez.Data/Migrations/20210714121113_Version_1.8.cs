using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BullBeez.Data.Migrations
{
    public partial class Version_18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyAndPersonProject_CompanyAndPerson_CompanyAndPersonId",
                table: "CompanyAndPersonProject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyAndPersonProject",
                table: "CompanyAndPersonProject");

            migrationBuilder.RenameColumn(
                name: "CompanyAndPersonId",
                table: "CompanyAndPersonProject",
                newName: "ComponyAndPersonId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CompanyAndPersonProject",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "InsertedBy",
                table: "CompanyAndPersonProject",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "InsertedDate",
                table: "CompanyAndPersonProject",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsertedIp",
                table: "CompanyAndPersonProject",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "RowStatu",
                table: "CompanyAndPersonProject",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CompanyAndPersonProject",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "CompanyAndPersonProject",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedIp",
                table: "CompanyAndPersonProject",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyAndPersonProject",
                table: "CompanyAndPersonProject",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAndPersonProject_ComponyAndPersonId",
                table: "CompanyAndPersonProject",
                column: "ComponyAndPersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyAndPersonProject_CompanyAndPerson_ComponyAndPersonId",
                table: "CompanyAndPersonProject",
                column: "ComponyAndPersonId",
                principalTable: "CompanyAndPerson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyAndPersonProject_CompanyAndPerson_ComponyAndPersonId",
                table: "CompanyAndPersonProject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyAndPersonProject",
                table: "CompanyAndPersonProject");

            migrationBuilder.DropIndex(
                name: "IX_CompanyAndPersonProject_ComponyAndPersonId",
                table: "CompanyAndPersonProject");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CompanyAndPersonProject");

            migrationBuilder.DropColumn(
                name: "InsertedBy",
                table: "CompanyAndPersonProject");

            migrationBuilder.DropColumn(
                name: "InsertedDate",
                table: "CompanyAndPersonProject");

            migrationBuilder.DropColumn(
                name: "InsertedIp",
                table: "CompanyAndPersonProject");

            migrationBuilder.DropColumn(
                name: "RowStatu",
                table: "CompanyAndPersonProject");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CompanyAndPersonProject");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "CompanyAndPersonProject");

            migrationBuilder.DropColumn(
                name: "UpdatedIp",
                table: "CompanyAndPersonProject");

            migrationBuilder.RenameColumn(
                name: "ComponyAndPersonId",
                table: "CompanyAndPersonProject",
                newName: "CompanyAndPersonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyAndPersonProject",
                table: "CompanyAndPersonProject",
                columns: new[] { "CompanyAndPersonId", "ProjectId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyAndPersonProject_CompanyAndPerson_CompanyAndPersonId",
                table: "CompanyAndPersonProject",
                column: "CompanyAndPersonId",
                principalTable: "CompanyAndPerson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
