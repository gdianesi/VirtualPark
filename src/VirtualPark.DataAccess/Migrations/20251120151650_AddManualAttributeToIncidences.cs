using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualPark.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddManualAttributeToIncidences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ManualOverride",
                table: "Incidences",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManualOverride",
                table: "Incidences");
        }
    }
}
