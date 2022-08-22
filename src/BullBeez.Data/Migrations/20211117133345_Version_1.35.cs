﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace BullBeez.Data.Migrations
{
    public partial class Version_135 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UniqCode",
                table: "Packages",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqCode",
                table: "Packages");
        }
    }
}
