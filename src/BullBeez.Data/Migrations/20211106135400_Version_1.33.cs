using Microsoft.EntityFrameworkCore.Migrations;

namespace BullBeez.Data.Migrations
{
    public partial class Version_133 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserIdWhoLike",
                table: "UserPosts",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UserIdWhoLike",
                table: "PostComments",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserIdWhoLike",
                table: "UserPosts");

            migrationBuilder.DropColumn(
                name: "UserIdWhoLike",
                table: "PostComments");
        }
    }
}
