using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualPark.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addPermissiontoadmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Description", "Key" },
                values: new object[] { new Guid("88888888-1111-1111-1111-111111111114"), "Allows update clockApp", "UpdateClock-ClockApp" });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[] { new Guid("88888888-1111-1111-1111-111111111114"), new Guid("aaaa1111-1111-1111-1111-111111111111") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("88888888-1111-1111-1111-111111111114"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("88888888-1111-1111-1111-111111111114"));
        }
    }
}
