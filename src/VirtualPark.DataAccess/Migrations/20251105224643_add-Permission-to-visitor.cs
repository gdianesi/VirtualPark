using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualPark.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addPermissiontovisitor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[] { new Guid("10101010-1111-1111-1111-111111111111"), new Guid("cccc1111-1111-1111-1111-111111111111") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("10101010-1111-1111-1111-111111111111"), new Guid("cccc1111-1111-1111-1111-111111111111") });
        }
    }
}
