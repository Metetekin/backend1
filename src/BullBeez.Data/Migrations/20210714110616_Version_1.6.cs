using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BullBeez.Data.Migrations
{
    public partial class Version_16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyAndPersonSkill");

            migrationBuilder.CreateTable(
                name: "CompanyAndPersonSkills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ComponyAndPersonId = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_CompanyAndPersonSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyAndPersonSkills_CompanyAndPerson_ComponyAndPersonId",
                        column: x => x.ComponyAndPersonId,
                        principalTable: "CompanyAndPerson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyAndPersonSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAndPersonSkills_ComponyAndPersonId",
                table: "CompanyAndPersonSkills",
                column: "ComponyAndPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAndPersonSkills_SkillId",
                table: "CompanyAndPersonSkills",
                column: "SkillId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyAndPersonSkills");

            migrationBuilder.CreateTable(
                name: "CompanyAndPersonSkill",
                columns: table => new
                {
                    CompanyAndPersonId = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyAndPersonSkill", x => new { x.CompanyAndPersonId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_CompanyAndPersonSkill_CompanyAndPerson_CompanyAndPersonId",
                        column: x => x.CompanyAndPersonId,
                        principalTable: "CompanyAndPerson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyAndPersonSkill_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAndPersonSkill_SkillId",
                table: "CompanyAndPersonSkill",
                column: "SkillId");
        }
    }
}
