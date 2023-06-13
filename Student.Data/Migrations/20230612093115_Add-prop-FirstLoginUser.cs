using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudentRegistration.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddpropFirstLoginUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("40481e91-c97a-4abc-afc6-deb49a5e904c"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("7e5c5cc1-9ea8-4c3a-a2b0-a9de812b2a61"));

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: "1951060632");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("5156829f-36d4-47a8-88e4-c8ac6093b124"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ac8bedc6-01f4-4352-87c8-a4c288096a79"));

            migrationBuilder.AddColumn<bool>(
                name: "FirstLogin",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstLogin",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("40481e91-c97a-4abc-afc6-deb49a5e904c"), "ddca99bf-26fa-401f-b119-0f8abb7c9e4b", "Day la role student", "student", "student" },
                    { new Guid("7e5c5cc1-9ea8-4c3a-a2b0-a9de812b2a61"), "a485be5d-713a-47c5-9004-609a861f70b7", "Day la role admin", "admin", "admin" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("5156829f-36d4-47a8-88e4-c8ac6093b124"), 0, "c7b7870c-9edc-402f-82e5-2b8f52abfac9", "admin@example.com", true, false, null, "admin@example.com", "admin", "AQAAAAEAACcQAAAAEAAc4jz3oPm0OUIv548lmQSt/kSRd3T2I/uCxSNLyXegRlauNa0IgOo/Vq1RNIkJXw==", null, false, "", false, "admin" },
                    { new Guid("ac8bedc6-01f4-4352-87c8-a4c288096a79"), 0, "e53238e7-bb52-4f38-862e-f9584ba24588", "student@example.com", true, false, null, "student@example.com", "student", "AQAAAAEAACcQAAAAELH/8hjRGohGyhJe5Na7DCJk8BiAwWbrT2tr5H5sftsNqgjEHNvb/GtWManHgHol3Q==", "0947771450", false, "", false, "student" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "PhoneNumber", "UserId" },
                values: new object[] { "1951060632", "dungbt@gmail.com", "Dũng", "Bùi", "0947771450", new Guid("ac8bedc6-01f4-4352-87c8-a4c288096a79") });
        }
    }
}
