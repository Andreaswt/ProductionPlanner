using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductionPlanner.Data.Migrations
{
    public partial class _19dec1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Guid",
                table: "ProjectTask");

            migrationBuilder.AddColumn<int>(
                name: "ProjectTemplateId",
                table: "ProjectTask",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProjectTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTemplates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTask_ProjectTemplateId",
                table: "ProjectTask",
                column: "ProjectTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTask_ProjectTemplates_ProjectTemplateId",
                table: "ProjectTask",
                column: "ProjectTemplateId",
                principalTable: "ProjectTemplates",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTask_ProjectTemplates_ProjectTemplateId",
                table: "ProjectTask");

            migrationBuilder.DropTable(
                name: "ProjectTemplates");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTask_ProjectTemplateId",
                table: "ProjectTask");

            migrationBuilder.DropColumn(
                name: "ProjectTemplateId",
                table: "ProjectTask");

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "ProjectTask",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
