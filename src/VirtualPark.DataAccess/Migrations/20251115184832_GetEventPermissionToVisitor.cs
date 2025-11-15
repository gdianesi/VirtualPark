using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualPark.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class GetEventPermissionToVisitor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[] { new Guid("22222222-1111-1111-1111-111111111111"), new Guid("cccc1111-1111-1111-1111-111111111111") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("22222222-1111-1111-1111-111111111111"), new Guid("cccc1111-1111-1111-1111-111111111111") });
        }
    }
}
