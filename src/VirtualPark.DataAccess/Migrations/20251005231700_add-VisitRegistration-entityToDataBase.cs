using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualPark.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addVisitRegistrationentityToDataBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VisitRegistrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VisitorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitRegistrations_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitRegistrations_VisitorsProfile_VisitorId",
                        column: x => x.VisitorId,
                        principalTable: "VisitorsProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VisitRegistrationsAttractions",
                columns: table => new
                {
                    VisitRegistrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitRegistrationsAttractions", x => new { x.AttractionId, x.VisitRegistrationId });
                    table.ForeignKey(
                        name: "FK_VisitRegistrationsAttractions_Attractions_AttractionId",
                        column: x => x.AttractionId,
                        principalTable: "Attractions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitRegistrationsAttractions_VisitRegistrations_VisitRegistrationId",
                        column: x => x.VisitRegistrationId,
                        principalTable: "VisitRegistrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VisitRegistrations_TicketId",
                table: "VisitRegistrations",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitRegistrations_VisitorId",
                table: "VisitRegistrations",
                column: "VisitorId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitRegistrationsAttractions_AttractionId",
                table: "VisitRegistrationsAttractions",
                column: "AttractionId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitRegistrationsAttractions_VisitRegistrationId",
                table: "VisitRegistrationsAttractions",
                column: "VisitRegistrationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VisitRegistrationsAttractions");

            migrationBuilder.DropTable(
                name: "VisitRegistrations");
        }
    }
}
