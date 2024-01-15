using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace deha_exam_quanlykhoahoc.Data.Migrations
{
    public partial class mydb_v8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassDetail_Class_ClassID",
                table: "ClassDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassDetail_Users_UserID",
                table: "ClassDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_Lesson_Class_ClassID",
                table: "Lesson");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassDetail_Class_ClassID",
                table: "ClassDetail",
                column: "ClassID",
                principalTable: "Class",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassDetail_Users_UserID",
                table: "ClassDetail",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lesson_Class_ClassID",
                table: "Lesson",
                column: "ClassID",
                principalTable: "Class",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassDetail_Class_ClassID",
                table: "ClassDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassDetail_Users_UserID",
                table: "ClassDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_Lesson_Class_ClassID",
                table: "Lesson");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassDetail_Class_ClassID",
                table: "ClassDetail",
                column: "ClassID",
                principalTable: "Class",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassDetail_Users_UserID",
                table: "ClassDetail",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lesson_Class_ClassID",
                table: "Lesson",
                column: "ClassID",
                principalTable: "Class",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
