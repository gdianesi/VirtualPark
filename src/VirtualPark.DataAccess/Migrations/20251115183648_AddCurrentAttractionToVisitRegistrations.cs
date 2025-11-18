using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualPark.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrentAttractionToVisitRegistrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CurrentAttractionId",
                table: "VisitRegistrations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_VisitRegistrations_CurrentAttractionId",
                table: "VisitRegistrations",
                column: "CurrentAttractionId");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitRegistrations_Attractions_CurrentAttractionId",
                table: "VisitRegistrations",
                column: "CurrentAttractionId",
                principalTable: "Attractions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VisitRegistrations_Attractions_CurrentAttractionId",
                table: "VisitRegistrations");

            migrationBuilder.DropIndex(
                name: "IX_VisitRegistrations_CurrentAttractionId",
                table: "VisitRegistrations");

            migrationBuilder.DropColumn(
                name: "CurrentAttractionId",
                table: "VisitRegistrations");
        }
    }
}
