using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VirtualPark.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddRewardPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Description", "Key" },
                values: new object[,]
                {
                    { new Guid("55555555-1111-1111-1111-111111111111"), "Allows retrieving details of a specific reward", "GetRewardById-Reward" },
                    { new Guid("55555555-1111-1111-1111-111111111112"), "Allows creating new rewards", "CreateReward-Reward" },
                    { new Guid("55555555-1111-1111-1111-111111111113"), "Allows listing all rewards", "GetAllRewards-Reward" },
                    { new Guid("55555555-1111-1111-1111-111111111114"), "Allows updating rewards", "UpdateReward-Reward" },
                    { new Guid("55555555-1111-1111-1111-111111111115"), "Allows deleting rewards", "DeleteReward-Reward" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("55555555-1111-1111-1111-111111111111"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("55555555-1111-1111-1111-111111111112"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("55555555-1111-1111-1111-111111111113"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("55555555-1111-1111-1111-111111111114"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("55555555-1111-1111-1111-111111111115"), new Guid("aaaa1111-1111-1111-1111-111111111111") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("55555555-1111-1111-1111-111111111111"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("55555555-1111-1111-1111-111111111112"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("55555555-1111-1111-1111-111111111113"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("55555555-1111-1111-1111-111111111114"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("55555555-1111-1111-1111-111111111115"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("55555555-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("55555555-1111-1111-1111-111111111112"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("55555555-1111-1111-1111-111111111113"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("55555555-1111-1111-1111-111111111114"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("55555555-1111-1111-1111-111111111115"));
        }
    }
}
