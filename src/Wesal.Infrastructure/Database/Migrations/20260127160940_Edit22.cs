using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wesal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Edit22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "ObligationAlerts",
                newName: "ViolationType");

            migrationBuilder.CreateTable(
                name: "ComplianceMetrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourtId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FamilyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalVisitationsScheduled = table.Column<int>(type: "int", nullable: false),
                    TotalVisitationsCompleted = table.Column<int>(type: "int", nullable: false),
                    TotalAlimonyPaymentsDue = table.Column<int>(type: "int", nullable: false),
                    TotalAlimonyPaymentsOnTime = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplianceMetrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplianceMetrics_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Families",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ComplianceMetrics_FamilyCourts_CourtId",
                        column: x => x.CourtId,
                        principalTable: "FamilyCourts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ComplianceMetrics_Parents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComplianceMetrics_CourtId",
                table: "ComplianceMetrics",
                column: "CourtId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplianceMetrics_FamilyId",
                table: "ComplianceMetrics",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplianceMetrics_ParentId",
                table: "ComplianceMetrics",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComplianceMetrics");

            migrationBuilder.RenameColumn(
                name: "ViolationType",
                table: "ObligationAlerts",
                newName: "Type");
        }
    }
}
