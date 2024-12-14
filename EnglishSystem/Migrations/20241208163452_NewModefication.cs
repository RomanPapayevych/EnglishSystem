using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishSystem.Migrations
{
    /// <inheritdoc />
    public partial class NewModefication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Schedules_GroupId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "Schedules");

            migrationBuilder.AddColumn<string>(
                name: "DaysOfWeek",
                table: "Schedules",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Schedules",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "Schedules",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_GroupId",
                table: "Schedules",
                column: "GroupId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Schedules_GroupId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "DaysOfWeek",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Schedules");

            migrationBuilder.AddColumn<int>(
                name: "DayOfWeek",
                table: "Schedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_GroupId",
                table: "Schedules",
                column: "GroupId");
        }
    }
}
