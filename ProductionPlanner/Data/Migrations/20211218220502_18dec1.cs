using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductionPlanner.Data.Migrations
{
    public partial class _18dec1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Day_Weeks_WeekId1",
                table: "Day");

            migrationBuilder.DropIndex(
                name: "IX_Day_WeekId1",
                table: "Day");

            migrationBuilder.DropColumn(
                name: "WeekId1",
                table: "Day");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WeekId1",
                table: "Day",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Day_WeekId1",
                table: "Day",
                column: "WeekId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Day_Weeks_WeekId1",
                table: "Day",
                column: "WeekId1",
                principalTable: "Weeks",
                principalColumn: "Id");
        }
    }
}
