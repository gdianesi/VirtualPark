using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VirtualPark.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addOperatorPermissionstoDeleteAndUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("44444444-1111-1111-1111-111111111114"), new Guid("bbbb1111-1111-1111-1111-111111111111") },
                    { new Guid("44444444-1111-1111-1111-111111111115"), new Guid("bbbb1111-1111-1111-1111-111111111111") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("44444444-1111-1111-1111-111111111114"), new Guid("bbbb1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("44444444-1111-1111-1111-111111111115"), new Guid("bbbb1111-1111-1111-1111-111111111111") });
        }
    }
}
