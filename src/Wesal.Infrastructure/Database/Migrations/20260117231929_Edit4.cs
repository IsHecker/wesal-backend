using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wesal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Edit4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "VisitationLocations");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "ClosingTime",
                table: "VisitationLocations",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<Guid>(
                name: "CourtId",
                table: "VisitationLocations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "OpeningTime",
                table: "VisitationLocations",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.CreateIndex(
                name: "IX_VisitationLocations_CourtId",
                table: "VisitationLocations",
                column: "CourtId");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitationLocations_FamilyCourts_CourtId",
                table: "VisitationLocations",
                column: "CourtId",
                principalTable: "FamilyCourts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VisitationLocations_FamilyCourts_CourtId",
                table: "VisitationLocations");

            migrationBuilder.DropIndex(
                name: "IX_VisitationLocations_CourtId",
                table: "VisitationLocations");

            migrationBuilder.DropColumn(
                name: "ClosingTime",
                table: "VisitationLocations");

            migrationBuilder.DropColumn(
                name: "CourtId",
                table: "VisitationLocations");

            migrationBuilder.DropColumn(
                name: "OpeningTime",
                table: "VisitationLocations");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "VisitationLocations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
