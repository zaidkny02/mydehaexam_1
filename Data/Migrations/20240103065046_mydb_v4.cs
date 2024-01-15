using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace deha_exam_quanlykhoahoc.Data.Migrations
{
    public partial class mydb_v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "FileinLesson",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "FileinLesson");
        }
    }
}
