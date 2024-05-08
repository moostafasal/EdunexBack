using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduNexDB.Migrations
{
    /// <inheritdoc />
    public partial class createdatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: 1,
                column: "LevelName",
                value: "الصف الأول الثانوي");

            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: 2,
                column: "LevelName",
                value: "الصف الثاني الثانوي");

            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: 3,
                column: "LevelName",
                value: "الصف الثالث الثانوي");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: 1,
                column: "LevelName",
                value: "Level one");

            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: 2,
                column: "LevelName",
                value: "Level two");

            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: 3,
                column: "LevelName",
                value: "Level three");
        }
    }
}
