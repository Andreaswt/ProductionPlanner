using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductionPlanner.Migrations
{
    public partial class ProjectTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTask_Day_DayId",
                table: "ProjectTask");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTask_Projects_ProjectId",
                table: "ProjectTask");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTask_ProjectTemplates_ProjectTemplateId",
                table: "ProjectTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectTask",
                table: "ProjectTask");

            migrationBuilder.RenameTable(
                name: "ProjectTask",
                newName: "ProjectTasks");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTask_ProjectTemplateId",
                table: "ProjectTasks",
                newName: "IX_ProjectTasks_ProjectTemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTask_ProjectId",
                table: "ProjectTasks",
                newName: "IX_ProjectTasks_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTask_DayId",
                table: "ProjectTasks",
                newName: "IX_ProjectTasks_DayId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectTasks",
                table: "ProjectTasks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_Day_DayId",
                table: "ProjectTasks",
                column: "DayId",
                principalTable: "Day",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_Projects_ProjectId",
                table: "ProjectTasks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_ProjectTemplates_ProjectTemplateId",
                table: "ProjectTasks",
                column: "ProjectTemplateId",
                principalTable: "ProjectTemplates",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_Day_DayId",
                table: "ProjectTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_Projects_ProjectId",
                table: "ProjectTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_ProjectTemplates_ProjectTemplateId",
                table: "ProjectTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectTasks",
                table: "ProjectTasks");

            migrationBuilder.RenameTable(
                name: "ProjectTasks",
                newName: "ProjectTask");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTasks_ProjectTemplateId",
                table: "ProjectTask",
                newName: "IX_ProjectTask_ProjectTemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTasks_ProjectId",
                table: "ProjectTask",
                newName: "IX_ProjectTask_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTasks_DayId",
                table: "ProjectTask",
                newName: "IX_ProjectTask_DayId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectTask",
                table: "ProjectTask",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTask_Day_DayId",
                table: "ProjectTask",
                column: "DayId",
                principalTable: "Day",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTask_Projects_ProjectId",
                table: "ProjectTask",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTask_ProjectTemplates_ProjectTemplateId",
                table: "ProjectTask",
                column: "ProjectTemplateId",
                principalTable: "ProjectTemplates",
                principalColumn: "Id");
        }
    }
}
