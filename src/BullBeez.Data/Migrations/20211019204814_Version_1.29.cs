using Microsoft.EntityFrameworkCore.Migrations;

namespace BullBeez.Data.Migrations
{
    public partial class Version_129 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "PostComments");

            migrationBuilder.AddColumn<int>(
                name: "CompanyAndPersonId",
                table: "UserPosts",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "PostComments",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "CompanyAndPersonId",
                table: "PostComments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPosts_CompanyAndPersonId",
                table: "UserPosts",
                column: "CompanyAndPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PostComments_CompanyAndPersonId",
                table: "PostComments",
                column: "CompanyAndPersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostComments_CompanyAndPerson_CompanyAndPersonId",
                table: "PostComments",
                column: "CompanyAndPersonId",
                principalTable: "CompanyAndPerson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPosts_CompanyAndPerson_CompanyAndPersonId",
                table: "UserPosts",
                column: "CompanyAndPersonId",
                principalTable: "CompanyAndPerson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostComments_CompanyAndPerson_CompanyAndPersonId",
                table: "PostComments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPosts_CompanyAndPerson_CompanyAndPersonId",
                table: "UserPosts");

            migrationBuilder.DropIndex(
                name: "IX_UserPosts_CompanyAndPersonId",
                table: "UserPosts");

            migrationBuilder.DropIndex(
                name: "IX_PostComments_CompanyAndPersonId",
                table: "PostComments");

            migrationBuilder.DropColumn(
                name: "CompanyAndPersonId",
                table: "UserPosts");

            migrationBuilder.DropColumn(
                name: "CompanyAndPersonId",
                table: "PostComments");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "PostComments",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "PostComments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
