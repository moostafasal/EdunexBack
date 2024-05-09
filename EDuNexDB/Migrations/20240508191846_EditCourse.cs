using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduNexDB.Migrations
{
    /// <inheritdoc />
    public partial class EditCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseType",
                table: "Courses");

            migrationBuilder.AddColumn<string>(
                name: "SubjectType",
                table: "Subjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubjectType",
                table: "Subjects");

            migrationBuilder.AddColumn<int>(
                name: "CourseType",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
