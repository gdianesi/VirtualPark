using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VirtualPark.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddPermissionToGetAndRestoreRewards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Description", "Key" },
                values: new object[,]
                {
                    { new Guid("55555555-1111-1111-1111-111111111116"), "Allows listing deleted rewards", "GetDeletedRewards-Reward" },
                    { new Guid("55555555-1111-1111-1111-111111111117"), "Allows restore a deleted reward", "RestoreReward-Reward" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("55555555-1111-1111-1111-111111111116"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("55555555-1111-1111-1111-111111111117"), new Guid("aaaa1111-1111-1111-1111-111111111111") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("55555555-1111-1111-1111-111111111116"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("55555555-1111-1111-1111-111111111117"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("55555555-1111-1111-1111-111111111116"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("55555555-1111-1111-1111-111111111117"));
        }
    }
}
