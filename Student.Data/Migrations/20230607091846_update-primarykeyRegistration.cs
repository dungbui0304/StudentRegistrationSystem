using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudentRegistration.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateprimarykeyRegistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Registrations",
                table: "Registrations");

            migrationBuilder.DeleteData(
                table: "Registrations",
                keyColumns: new[] { "CourseId", "StudentId" },
                keyValues: new object[] { "1", "1951060632" });

            migrationBuilder.DeleteData(
                table: "Registrations",
                keyColumns: new[] { "CourseId", "StudentId" },
                keyValues: new object[] { "2", "1951060632" });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("85c9890e-9af6-4302-982a-08cc0481b5e2"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("db016fe3-9be5-46bd-ad65-eb54643b67a3"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("9ea26fcc-6820-4fb0-9a98-db2e5299db2b"));

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: "1951060632");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("292a3f23-a20d-4db8-811e-efacbcf51c4d"));

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Registrations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Registrations_StudentId_CourseId",
                table: "Registrations",
                columns: new[] { "StudentId", "CourseId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Registrations",
                table: "Registrations",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Registrations_StudentId_CourseId",
                table: "Registrations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Registrations",
                table: "Registrations");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Registrations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Registrations",
                table: "Registrations",
                columns: new[] { "StudentId", "CourseId" });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { "1", "Khó quá ạ", "Toán" },
                    { "2", "Dài quá ạ", "Văn" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("85c9890e-9af6-4302-982a-08cc0481b5e2"), "d84b6207-3884-400c-8528-7c32cc848bff", "Day la role student", "student", "student" },
                    { new Guid("db016fe3-9be5-46bd-ad65-eb54643b67a3"), "aa931d30-4360-4bb9-ac99-b0cc1c8ef7e1", "Day la role admin", "admin", "admin" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("292a3f23-a20d-4db8-811e-efacbcf51c4d"), 0, "c9b06990-92cb-4716-8602-6af6cce7f67f", "student@example.com", true, false, null, "student@example.com", "student", "AQAAAAEAACcQAAAAEIHoHeTeJOsVCvx0kB9Ob8cuJXsHgbDAcEUTQKJTqWVB81S6ILbsvnjQERK01MNU0Q==", null, false, "", false, "student" },
                    { new Guid("9ea26fcc-6820-4fb0-9a98-db2e5299db2b"), 0, "22c9ee4c-42af-4dae-8e68-dbba80cf30e3", "admin@example.com", true, false, null, "admin@example.com", "admin", "AQAAAAEAACcQAAAAEAJ6Qi9Gv0Bi5Dmwlr6J2QGAz3CKLgshfm2UlgTicfFXuPekF8vb7aHs1I9oSJs04w==", null, false, "", false, "admin" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "PhoneNumber", "UserId" },
                values: new object[] { "1951060632", "dungbt@gmail.com", "Dũng", "Bùi", "0947771450", new Guid("292a3f23-a20d-4db8-811e-efacbcf51c4d") });

            migrationBuilder.InsertData(
                table: "Registrations",
                columns: new[] { "CourseId", "StudentId", "CreateAt", "Id" },
                values: new object[,]
                {
                    { "1", "1951060632", new DateTime(2023, 6, 2, 10, 53, 5, 467, DateTimeKind.Local).AddTicks(9609), "1" },
                    { "2", "1951060632", new DateTime(2023, 6, 2, 10, 53, 5, 467, DateTimeKind.Local).AddTicks(9619), "2" }
                });
        }
    }
}
