using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VirtualPark.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("10101010-1111-1111-1111-111111111111"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("10101010-1111-1111-1111-111111111112"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("10101010-1111-1111-1111-111111111113"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("10101010-1111-1111-1111-111111111114"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("10101010-1111-1111-1111-111111111115"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111112"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111113"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111114"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111115"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("12121212-1111-1111-1111-111111111111"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("12121212-1111-1111-1111-111111111112"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("13131313-1111-1111-1111-111111111111"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("13131313-1111-1111-1111-111111111112"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("13131313-1111-1111-1111-111111111113"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("13131313-1111-1111-1111-111111111114"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("13131313-1111-1111-1111-111111111115"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("22222222-1111-1111-1111-111111111111"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("22222222-1111-1111-1111-111111111112"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("22222222-1111-1111-1111-111111111113"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("22222222-1111-1111-1111-111111111114"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("22222222-1111-1111-1111-111111111115"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("33333333-1111-1111-1111-111111111111"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("33333333-1111-1111-1111-111111111112"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("33333333-1111-1111-1111-111111111113"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("33333333-1111-1111-1111-111111111114"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("44444444-1111-1111-1111-111111111111"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("44444444-1111-1111-1111-111111111112"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("44444444-1111-1111-1111-111111111113"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("44444444-1111-1111-1111-111111111114"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("44444444-1111-1111-1111-111111111115"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("66666666-1111-1111-1111-111111111111"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("66666666-1111-1111-1111-111111111112"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("66666666-1111-1111-1111-111111111113"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("66666666-1111-1111-1111-111111111114"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("66666666-1111-1111-1111-111111111115"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("77777777-1111-1111-1111-111111111111"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("77777777-1111-1111-1111-111111111112"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("77777777-1111-1111-1111-111111111113"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("77777777-1111-1111-1111-111111111114"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("77777777-1111-1111-1111-111111111115"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("88888888-1111-1111-1111-111111111111"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("88888888-1111-1111-1111-111111111112"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("99999999-1111-1111-1111-111111111111"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("99999999-1111-1111-1111-111111111112"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("99999999-1111-1111-1111-111111111113"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("99999999-1111-1111-1111-111111111114"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("99999999-1111-1111-1111-111111111115"), new Guid("aaaa1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111112"), new Guid("bbbb1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111113"), new Guid("bbbb1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111114"), new Guid("bbbb1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("11111111-1111-1111-1111-111111111115"), new Guid("bbbb1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("22222222-1111-1111-1111-111111111113"), new Guid("bbbb1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("22222222-1111-1111-1111-111111111114"), new Guid("bbbb1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("44444444-1111-1111-1111-111111111111"), new Guid("bbbb1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("44444444-1111-1111-1111-111111111112"), new Guid("bbbb1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("66666666-1111-1111-1111-111111111111"), new Guid("bbbb1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("66666666-1111-1111-1111-111111111112"), new Guid("bbbb1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("66666666-1111-1111-1111-111111111113"), new Guid("bbbb1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("66666666-1111-1111-1111-111111111115"), new Guid("bbbb1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("12121212-1111-1111-1111-111111111111"), new Guid("cccc1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("12121212-1111-1111-1111-111111111112"), new Guid("cccc1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("33333333-1111-1111-1111-111111111111"), new Guid("cccc1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("33333333-1111-1111-1111-111111111112"), new Guid("cccc1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("88888888-1111-1111-1111-111111111111"), new Guid("cccc1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("88888888-1111-1111-1111-111111111112"), new Guid("cccc1111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("10101010-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("10101010-1111-1111-1111-111111111112"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("10101010-1111-1111-1111-111111111113"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("10101010-1111-1111-1111-111111111114"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("10101010-1111-1111-1111-111111111115"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111112"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111113"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111114"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111115"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("12121212-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("12121212-1111-1111-1111-111111111112"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("13131313-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("13131313-1111-1111-1111-111111111112"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("13131313-1111-1111-1111-111111111113"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("13131313-1111-1111-1111-111111111114"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("13131313-1111-1111-1111-111111111115"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-1111-1111-1111-111111111112"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-1111-1111-1111-111111111113"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-1111-1111-1111-111111111114"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("22222222-1111-1111-1111-111111111115"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("33333333-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("33333333-1111-1111-1111-111111111112"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("33333333-1111-1111-1111-111111111113"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("33333333-1111-1111-1111-111111111114"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("44444444-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("44444444-1111-1111-1111-111111111112"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("44444444-1111-1111-1111-111111111113"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("44444444-1111-1111-1111-111111111114"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("44444444-1111-1111-1111-111111111115"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("66666666-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("66666666-1111-1111-1111-111111111112"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("66666666-1111-1111-1111-111111111113"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("66666666-1111-1111-1111-111111111114"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("66666666-1111-1111-1111-111111111115"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("77777777-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("77777777-1111-1111-1111-111111111112"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("77777777-1111-1111-1111-111111111113"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("77777777-1111-1111-1111-111111111114"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("77777777-1111-1111-1111-111111111115"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("88888888-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("88888888-1111-1111-1111-111111111112"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("99999999-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("99999999-1111-1111-1111-111111111112"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("99999999-1111-1111-1111-111111111113"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("99999999-1111-1111-1111-111111111114"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("99999999-1111-1111-1111-111111111115"));

            /*migrationBuilder.AddColumn<int>(
                name: "DailyScore",
                table: "VisitRegistrations",
                type: "int",
                nullable: false,
                defaultValue: 0);*/

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "LastName", "Name", "Password", "VisitorProfileId" },
                values: new object[] { new Guid("aaaa9999-1111-1111-1111-111111111111"), "admin@virtualpark.com", "System", "Admin", "Admin123!", null });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("aaaa1111-1111-1111-1111-111111111111"), new Guid("aaaa9999-1111-1111-1111-111111111111") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("aaaa1111-1111-1111-1111-111111111111"), new Guid("aaaa9999-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aaaa9999-1111-1111-1111-111111111111"));

            /*migrationBuilder.DropColumn(
                name: "DailyScore",
                table: "VisitRegistrations");*/

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Description", "Key" },
                values: new object[,]
                {
                    { new Guid("10101010-1111-1111-1111-111111111111"), "Allows retrieving details of a specific user", "GetUserById-User" },
                    { new Guid("10101010-1111-1111-1111-111111111112"), "Allows listing all users", "GetAllUsers-User" },
                    { new Guid("10101010-1111-1111-1111-111111111113"), "Allows updating user data", "UpdateUser-User" },
                    { new Guid("10101010-1111-1111-1111-111111111114"), "Allows deleting a user", "DeleteUser-User" },
                    { new Guid("10101010-1111-1111-1111-111111111115"), "Allows creating new users", "CreateUser-User" },
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Allows retrieving details of a specific attraction", "GetAttractionById-Attraction" },
                    { new Guid("11111111-1111-1111-1111-111111111112"), "Allows creating attractions", "CreateAttraction-Attraction" },
                    { new Guid("11111111-1111-1111-1111-111111111113"), "Allows listing all attractions", "GetAllAttractions-Attraction" },
                    { new Guid("11111111-1111-1111-1111-111111111114"), "Allows updating attraction data", "UpdateAttraction-Attraction" },
                    { new Guid("11111111-1111-1111-1111-111111111115"), "Allows deleting an attraction", "DeleteAttraction-Attraction" },
                    { new Guid("12121212-1111-1111-1111-111111111111"), "Allows retrieving visitor profile", "GetVisitorProfileById-VisitorProfile" },
                    { new Guid("12121212-1111-1111-1111-111111111112"), "Allows updating visitor profile", "UpdateVisitorProfile-VisitorProfile" },
                    { new Guid("13131313-1111-1111-1111-111111111111"), "Allows retrieving a strategy", "GetStrategyById-Strategy" },
                    { new Guid("13131313-1111-1111-1111-111111111112"), "Allows listing all strategies", "GetAllStrategies-Strategy" },
                    { new Guid("13131313-1111-1111-1111-111111111113"), "Allows creating strategies", "CreateStrategy-Strategy" },
                    { new Guid("13131313-1111-1111-1111-111111111114"), "Allows updating strategies", "UpdateStrategy-Strategy" },
                    { new Guid("13131313-1111-1111-1111-111111111115"), "Allows deleting strategies", "DeleteStrategy-Strategy" },
                    { new Guid("22222222-1111-1111-1111-111111111111"), "Allows retrieving details of a specific event", "GetEventById-Event" },
                    { new Guid("22222222-1111-1111-1111-111111111112"), "Allows creating events", "CreateEvent-Event" },
                    { new Guid("22222222-1111-1111-1111-111111111113"), "Allows listing all events", "GetAllEvents-Event" },
                    { new Guid("22222222-1111-1111-1111-111111111114"), "Allows updating event data", "UpdateEvent-Event" },
                    { new Guid("22222222-1111-1111-1111-111111111115"), "Allows deleting an event", "DeleteEvent-Event" },
                    { new Guid("33333333-1111-1111-1111-111111111111"), "Allows retrieving a ticket by ID", "GetTicketById-Ticket" },
                    { new Guid("33333333-1111-1111-1111-111111111112"), "Allows creating tickets", "CreateTicket-Ticket" },
                    { new Guid("33333333-1111-1111-1111-111111111113"), "Allows listing all tickets", "GetAllTickets-Ticket" },
                    { new Guid("33333333-1111-1111-1111-111111111114"), "Allows deleting a ticket", "DeleteTicket-Ticket" },
                    { new Guid("44444444-1111-1111-1111-111111111111"), "Allows retrieving a type incidence", "GetTypeIncidenceById-TypeIncidence" },
                    { new Guid("44444444-1111-1111-1111-111111111112"), "Allows listing all type incidences", "GetAllTypeIncidences-TypeIncidence" },
                    { new Guid("44444444-1111-1111-1111-111111111113"), "Allows creating type incidences", "CreateTypeIncidence-TypeIncidence" },
                    { new Guid("44444444-1111-1111-1111-111111111114"), "Allows updating type incidences", "UpdateTypeIncidence-TypeIncidence" },
                    { new Guid("44444444-1111-1111-1111-111111111115"), "Allows deleting type incidences", "DeleteTypeIncidence-TypeIncidence" },
                    { new Guid("66666666-1111-1111-1111-111111111111"), "Allows retrieving an incidence", "GetIncidenceById-Incidence" },
                    { new Guid("66666666-1111-1111-1111-111111111112"), "Allows creating incidences", "CreateIncidence-Incidence" },
                    { new Guid("66666666-1111-1111-1111-111111111113"), "Allows listing incidences", "GetAllIncidences-Incidence" },
                    { new Guid("66666666-1111-1111-1111-111111111114"), "Allows updating incidence info", "UpdateIncidence-Incidence" },
                    { new Guid("66666666-1111-1111-1111-111111111115"), "Allows deleting incidences", "DeleteIncidence-Incidence" },
                    { new Guid("77777777-1111-1111-1111-111111111111"), "Allows retrieving a permission", "GetPermissionById-Permission" },
                    { new Guid("77777777-1111-1111-1111-111111111112"), "Allows creating permissions", "CreatePermission-Permission" },
                    { new Guid("77777777-1111-1111-1111-111111111113"), "Allows listing permissions", "GetAllPermissions-Permission" },
                    { new Guid("77777777-1111-1111-1111-111111111114"), "Allows updating permissions", "UpdatePermission-Permission" },
                    { new Guid("77777777-1111-1111-1111-111111111115"), "Allows deleting permissions", "DeletePermission-Permission" },
                    { new Guid("88888888-1111-1111-1111-111111111111"), "Allows retrieving ranking by period", "GetRankingByPeriod-Ranking" },
                    { new Guid("88888888-1111-1111-1111-111111111112"), "Allows listing all rankings", "GetAllRankings-Ranking" },
                    { new Guid("99999999-1111-1111-1111-111111111111"), "Allows retrieving a role", "GetRoleById-Role" },
                    { new Guid("99999999-1111-1111-1111-111111111112"), "Allows creating roles", "CreateRole-Role" },
                    { new Guid("99999999-1111-1111-1111-111111111113"), "Allows listing roles", "GetAllRoles-Role" },
                    { new Guid("99999999-1111-1111-1111-111111111114"), "Allows updating roles", "UpdateRole-Role" },
                    { new Guid("99999999-1111-1111-1111-111111111115"), "Allows deleting roles", "DeleteRole-Role" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("10101010-1111-1111-1111-111111111111"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("10101010-1111-1111-1111-111111111112"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("10101010-1111-1111-1111-111111111113"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("10101010-1111-1111-1111-111111111114"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("10101010-1111-1111-1111-111111111115"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("11111111-1111-1111-1111-111111111112"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("11111111-1111-1111-1111-111111111113"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("11111111-1111-1111-1111-111111111114"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("11111111-1111-1111-1111-111111111115"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("12121212-1111-1111-1111-111111111111"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("12121212-1111-1111-1111-111111111112"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("13131313-1111-1111-1111-111111111111"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("13131313-1111-1111-1111-111111111112"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("13131313-1111-1111-1111-111111111113"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("13131313-1111-1111-1111-111111111114"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("13131313-1111-1111-1111-111111111115"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("22222222-1111-1111-1111-111111111111"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("22222222-1111-1111-1111-111111111112"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("22222222-1111-1111-1111-111111111113"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("22222222-1111-1111-1111-111111111114"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("22222222-1111-1111-1111-111111111115"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("33333333-1111-1111-1111-111111111111"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("33333333-1111-1111-1111-111111111112"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("33333333-1111-1111-1111-111111111113"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("33333333-1111-1111-1111-111111111114"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("44444444-1111-1111-1111-111111111111"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("44444444-1111-1111-1111-111111111112"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("44444444-1111-1111-1111-111111111113"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("44444444-1111-1111-1111-111111111114"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("44444444-1111-1111-1111-111111111115"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("66666666-1111-1111-1111-111111111111"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("66666666-1111-1111-1111-111111111112"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("66666666-1111-1111-1111-111111111113"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("66666666-1111-1111-1111-111111111114"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("66666666-1111-1111-1111-111111111115"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("77777777-1111-1111-1111-111111111111"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("77777777-1111-1111-1111-111111111112"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("77777777-1111-1111-1111-111111111113"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("77777777-1111-1111-1111-111111111114"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("77777777-1111-1111-1111-111111111115"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("88888888-1111-1111-1111-111111111111"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("88888888-1111-1111-1111-111111111112"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("99999999-1111-1111-1111-111111111111"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("99999999-1111-1111-1111-111111111112"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("99999999-1111-1111-1111-111111111113"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("99999999-1111-1111-1111-111111111114"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("99999999-1111-1111-1111-111111111115"), new Guid("aaaa1111-1111-1111-1111-111111111111") },
                    { new Guid("11111111-1111-1111-1111-111111111112"), new Guid("bbbb1111-1111-1111-1111-111111111111") },
                    { new Guid("11111111-1111-1111-1111-111111111113"), new Guid("bbbb1111-1111-1111-1111-111111111111") },
                    { new Guid("11111111-1111-1111-1111-111111111114"), new Guid("bbbb1111-1111-1111-1111-111111111111") },
                    { new Guid("11111111-1111-1111-1111-111111111115"), new Guid("bbbb1111-1111-1111-1111-111111111111") },
                    { new Guid("22222222-1111-1111-1111-111111111113"), new Guid("bbbb1111-1111-1111-1111-111111111111") },
                    { new Guid("22222222-1111-1111-1111-111111111114"), new Guid("bbbb1111-1111-1111-1111-111111111111") },
                    { new Guid("44444444-1111-1111-1111-111111111111"), new Guid("bbbb1111-1111-1111-1111-111111111111") },
                    { new Guid("44444444-1111-1111-1111-111111111112"), new Guid("bbbb1111-1111-1111-1111-111111111111") },
                    { new Guid("66666666-1111-1111-1111-111111111111"), new Guid("bbbb1111-1111-1111-1111-111111111111") },
                    { new Guid("66666666-1111-1111-1111-111111111112"), new Guid("bbbb1111-1111-1111-1111-111111111111") },
                    { new Guid("66666666-1111-1111-1111-111111111113"), new Guid("bbbb1111-1111-1111-1111-111111111111") },
                    { new Guid("66666666-1111-1111-1111-111111111115"), new Guid("bbbb1111-1111-1111-1111-111111111111") },
                    { new Guid("12121212-1111-1111-1111-111111111111"), new Guid("cccc1111-1111-1111-1111-111111111111") },
                    { new Guid("12121212-1111-1111-1111-111111111112"), new Guid("cccc1111-1111-1111-1111-111111111111") },
                    { new Guid("33333333-1111-1111-1111-111111111111"), new Guid("cccc1111-1111-1111-1111-111111111111") },
                    { new Guid("33333333-1111-1111-1111-111111111112"), new Guid("cccc1111-1111-1111-1111-111111111111") },
                    { new Guid("88888888-1111-1111-1111-111111111111"), new Guid("cccc1111-1111-1111-1111-111111111111") },
                    { new Guid("88888888-1111-1111-1111-111111111112"), new Guid("cccc1111-1111-1111-1111-111111111111") }
                });
        }
    }
}
