using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualPark.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddVisitScore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VisitScores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OccurredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    DayStrategyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VisitRegistrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitScores_VisitRegistrations_VisitRegistrationId",
                        column: x => x.VisitRegistrationId,
                        principalTable: "VisitRegistrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VisitScores_OccurredAt",
                table: "VisitScores",
                column: "OccurredAt");

            migrationBuilder.CreateIndex(
                name: "IX_VisitScores_Origin",
                table: "VisitScores",
                column: "Origin");

            migrationBuilder.CreateIndex(
                name: "IX_VisitScores_VisitRegistrationId",
                table: "VisitScores",
                column: "VisitRegistrationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VisitScores");
        }
    }
}
