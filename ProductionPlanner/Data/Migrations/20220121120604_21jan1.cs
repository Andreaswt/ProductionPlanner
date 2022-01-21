using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductionPlanner.Migrations
{
    public partial class _21jan1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Day_Weeks_WeekId",
                table: "Day");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_Day_DayId",
                table: "ProjectTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Day",
                table: "Day");

            migrationBuilder.RenameTable(
                name: "Day",
                newName: "Days");

            migrationBuilder.RenameIndex(
                name: "IX_Day_WeekId",
                table: "Days",
                newName: "IX_Days_WeekId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Days",
                table: "Days",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Days_Weeks_WeekId",
                table: "Days",
                column: "WeekId",
                principalTable: "Weeks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_Days_DayId",
                table: "ProjectTasks",
                column: "DayId",
                principalTable: "Days",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Days_Weeks_WeekId",
                table: "Days");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_Days_DayId",
                table: "ProjectTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Days",
                table: "Days");

            migrationBuilder.RenameTable(
                name: "Days",
                newName: "Day");

            migrationBuilder.RenameIndex(
                name: "IX_Days_WeekId",
                table: "Day",
                newName: "IX_Day_WeekId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Day",
                table: "Day",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Day_Weeks_WeekId",
                table: "Day",
                column: "WeekId",
                principalTable: "Weeks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_Day_DayId",
                table: "ProjectTasks",
                column: "DayId",
                principalTable: "Day",
                principalColumn: "Id");
        }
    }
}
