using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsValidEmail = table.Column<bool>(type: "bit", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RetryPassworkCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Administrator with full access.", "Admin", true, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Regular user with limited access.", "User", true, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manager with elevated access.", "Manager", true, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Guest user with minimal access.", "Guest", true, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "Avatar", "CreatedAt", "Email", "IsValidEmail", "Name", "Password", "Phone", "RefreshToken", "RefreshTokenExpiryTime", "RoleId", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), "https://example.com/avatar/admin.png", new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@gmail.com", false, "Admin User", "AQAAAAIAAYagAAAAECOKLm/4FZO24eI7K0UBk1sfvvKBDjkgVTBkpDsRrwhHuwODW6Z/VhbLoF7sJMdhjw==", "0123456789", null, null, new Guid("00000000-0000-0000-0000-000000000001"), true, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000002"), "https://example.com/avatar/user.png", new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "user@gmail.com", true, "Normal User", "AQAAAAIAAYagAAAAECOKLm/4FZO24eI7K0UBk1sfvvKBDjkgVTBkpDsRrwhHuwODW6Z/VhbLoF7sJMdhjw==", "0987654321", null, null, new Guid("00000000-0000-0000-0000-000000000002"), true, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_Email",
                table: "users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_Phone",
                table: "users",
                column: "Phone",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
