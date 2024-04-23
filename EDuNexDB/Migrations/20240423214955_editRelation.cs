using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EduNexDB.Migrations
{
    /// <inheritdoc />
    public partial class editRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Levels_LevelId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LevelId",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "LevelId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "LevelId",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_LevelId",
                table: "Students",
                column: "LevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Levels_LevelId",
                table: "Students",
                column: "LevelId",
                principalTable: "Levels",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Levels_LevelId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_LevelId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "LevelId",
                table: "Students");

            migrationBuilder.AddColumn<int>(
                name: "LevelId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Levels",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "IsDeleted", "LevelName", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Level one", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Level two", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Level three", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LevelId",
                table: "AspNetUsers",
                column: "LevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Levels_LevelId",
                table: "AspNetUsers",
                column: "LevelId",
                principalTable: "Levels",
                principalColumn: "Id");
        }
    }
}
