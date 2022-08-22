﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace BullBeez.Data.Migrations
{
    public partial class Version_145 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUpgradedToBoard",
                table: "UserPosts",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUpgradedToBoard",
                table: "UserPosts");
        }
    }
}
