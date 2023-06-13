using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudentRegistration.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("5c000e35-925d-4da8-a1eb-0d2f3f33e598"), "91bb4865-f050-4e44-93c6-840b2a378f54", "Day la role admin", "admin", "admin" },
                    { new Guid("e2ddfaef-f275-4b47-bae8-fe89e2f39d50"), "16ae4580-d743-4f2b-bb11-88881e3aa394", "Day la role student", "student", "student" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("e2ddfaef-f275-4b47-bae8-fe89e2f39d50"), new Guid("12298c93-d449-4e38-ac08-797cbad8f91a") },
                    { new Guid("5c000e35-925d-4da8-a1eb-0d2f3f33e598"), new Guid("89ad972f-6bfd-4676-bbb2-a5fe7fcd9250") }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstLogin", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("12298c93-d449-4e38-ac08-797cbad8f91a"), 0, "68f06a5a-65c8-47ca-bcf8-c8d4c4771f15", "student@example.com", true, true, false, null, "student@example.com", "student", "AQAAAAEAACcQAAAAENjtsuKrxZpJ428+F2E/SQAssLVCwVpdDIuZZh0Q2FsNkpDVP8AI2fc3O4NNBj0wiw==", "0947771450", false, "", false, "student" },
                    { new Guid("89ad972f-6bfd-4676-bbb2-a5fe7fcd9250"), 0, "675138c4-e2b6-4e0d-8ae5-3016f2359a5e", "admin@example.com", true, true, false, null, "admin@example.com", "admin", "AQAAAAEAACcQAAAAEPV3oksA3N0VG1U6XpWYiZJO1G8y9OIFkhRrohH/z1eHLUBN2mZnjG7oaaL0mC5prA==", null, false, "", false, "admin" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "PhoneNumber", "UserId" },
                values: new object[] { "1951060632", "dungbt@gmail.com", "Dũng", "Bùi", "0947771450", new Guid("12298c93-d449-4e38-ac08-797cbad8f91a") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("5c000e35-925d-4da8-a1eb-0d2f3f33e598"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("e2ddfaef-f275-4b47-bae8-fe89e2f39d50"));

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: "1951060632");

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("e2ddfaef-f275-4b47-bae8-fe89e2f39d50"), new Guid("12298c93-d449-4e38-ac08-797cbad8f91a") });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("5c000e35-925d-4da8-a1eb-0d2f3f33e598"), new Guid("89ad972f-6bfd-4676-bbb2-a5fe7fcd9250") });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("89ad972f-6bfd-4676-bbb2-a5fe7fcd9250"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("12298c93-d449-4e38-ac08-797cbad8f91a"));
        }
    }
}
