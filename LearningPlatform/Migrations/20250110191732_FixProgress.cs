using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningPlatform.Migrations
{
    /// <inheritdoc />
    public partial class FixProgress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Progresses_Lessons_LessonId",
                table: "Progresses");

            migrationBuilder.DropIndex(
                name: "IX_Progresses_LessonId",
                table: "Progresses");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "Progresses");

            migrationBuilder.CreateIndex(
                name: "IX_Progresses_RecentLessonId",
                table: "Progresses",
                column: "RecentLessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Progresses_Lessons_RecentLessonId",
                table: "Progresses",
                column: "RecentLessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Progresses_Lessons_RecentLessonId",
                table: "Progresses");

            migrationBuilder.DropIndex(
                name: "IX_Progresses_RecentLessonId",
                table: "Progresses");

            migrationBuilder.AddColumn<int>(
                name: "LessonId",
                table: "Progresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Progresses_LessonId",
                table: "Progresses",
                column: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Progresses_Lessons_LessonId",
                table: "Progresses",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
