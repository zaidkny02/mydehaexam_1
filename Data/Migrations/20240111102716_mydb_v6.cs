using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace deha_exam_quanlykhoahoc.Data.Migrations
{
    public partial class mydb_v6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Lesson_LessonID",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Users_UserID",
                table: "Comment");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Lesson_LessonID",
                table: "Comment",
                column: "LessonID",
                principalTable: "Lesson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Users_UserID",
                table: "Comment",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Lesson_LessonID",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Users_UserID",
                table: "Comment");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Lesson_LessonID",
                table: "Comment",
                column: "LessonID",
                principalTable: "Lesson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Users_UserID",
                table: "Comment",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
