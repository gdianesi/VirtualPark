using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualPark.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addActiveStrategyentityToDataBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActiveStrategies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StrategyKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveStrategies", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActiveStrategies_Date",
                table: "ActiveStrategies",
                column: "Date",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActiveStrategies");
        }
    }
}
