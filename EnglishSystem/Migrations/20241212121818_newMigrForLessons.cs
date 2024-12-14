using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishSystem.Migrations
{
    /// <inheritdoc />
    public partial class newMigrForLessons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Groups_GroupId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Homework_LessonId",
                table: "Homework");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "Lessons",
                newName: "ScheduleId");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_GroupId",
                table: "Lessons",
                newName: "IX_Lessons_ScheduleId");

            migrationBuilder.AddColumn<string>(
                name: "Topic",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Homework_LessonId",
                table: "Homework",
                column: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Schedules_ScheduleId",
                table: "Lessons",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Schedules_ScheduleId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Homework_LessonId",
                table: "Homework");

            migrationBuilder.DropColumn(
                name: "Topic",
                table: "Lessons");

            migrationBuilder.RenameColumn(
                name: "ScheduleId",
                table: "Lessons",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_ScheduleId",
                table: "Lessons",
                newName: "IX_Lessons_GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Homework_LessonId",
                table: "Homework",
                column: "LessonId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Groups_GroupId",
                table: "Lessons",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
