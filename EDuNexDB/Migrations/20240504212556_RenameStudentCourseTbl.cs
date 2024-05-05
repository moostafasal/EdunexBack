using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduNexDB.Migrations
{
    /// <inheritdoc />
    public partial class RenameStudentCourseTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_studentCourse_Courses_CourseId",
                table: "studentCourse");

            migrationBuilder.DropForeignKey(
                name: "FK_studentCourse_Students_StudentId",
                table: "studentCourse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_studentCourse",
                table: "studentCourse");

            migrationBuilder.RenameTable(
                name: "studentCourse",
                newName: "StudentCourse");

            migrationBuilder.RenameIndex(
                name: "IX_studentCourse_StudentId",
                table: "StudentCourse",
                newName: "IX_StudentCourse_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_studentCourse_CourseId",
                table: "StudentCourse",
                newName: "IX_StudentCourse_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentCourse",
                table: "StudentCourse",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourse_Courses_CourseId",
                table: "StudentCourse",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourse_Students_StudentId",
                table: "StudentCourse",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourse_Courses_CourseId",
                table: "StudentCourse");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourse_Students_StudentId",
                table: "StudentCourse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentCourse",
                table: "StudentCourse");

            migrationBuilder.RenameTable(
                name: "StudentCourse",
                newName: "studentCourse");

            migrationBuilder.RenameIndex(
                name: "IX_StudentCourse_StudentId",
                table: "studentCourse",
                newName: "IX_studentCourse_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentCourse_CourseId",
                table: "studentCourse",
                newName: "IX_studentCourse_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_studentCourse",
                table: "studentCourse",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_studentCourse_Courses_CourseId",
                table: "studentCourse",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_studentCourse_Students_StudentId",
                table: "studentCourse",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }
    }
}
