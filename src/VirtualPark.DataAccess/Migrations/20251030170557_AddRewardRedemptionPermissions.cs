using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VirtualPark.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddRewardRedemptionPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Description", "Key" },
                values: new object[,]
                {
                    { new Guid("56565656-1111-1111-1111-111111111111"), "Allows redeeming rewards", "RedeemReward-RewardRedemption" },
                    { new Guid("56565656-1111-1111-1111-111111111112"), "Allows retrieving a specific reward redemption", "GetRewardRedemptionById-RewardRedemption" },
                    { new Guid("56565656-1111-1111-1111-111111111113"), "Allows listing all reward redemptions", "GetAllRewardRedemptions-RewardRedemption" },
                    { new Guid("56565656-1111-1111-1111-111111111114"), "Allows retrieving redemptions of a specific visitor", "GetRewardRedemptionsByVisitor-RewardRedemption" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("56565656-1111-1111-1111-111111111111"), new Guid("cccc1111-1111-1111-1111-111111111111") },
                    { new Guid("56565656-1111-1111-1111-111111111114"), new Guid("cccc1111-1111-1111-1111-111111111111") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("56565656-1111-1111-1111-111111111112"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("56565656-1111-1111-1111-111111111113"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("56565656-1111-1111-1111-111111111111"), new Guid("cccc1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("56565656-1111-1111-1111-111111111114"), new Guid("cccc1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("56565656-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("56565656-1111-1111-1111-111111111114"));
        }
    }
}
