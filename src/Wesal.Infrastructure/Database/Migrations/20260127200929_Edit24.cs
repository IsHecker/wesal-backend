using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wesal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Edit24 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_VisitationSchedules_CourtId",
                table: "VisitationSchedules",
                column: "CourtId");

            migrationBuilder.CreateIndex(
                name: "IX_Alimonies_CourtId",
                table: "Alimonies",
                column: "CourtId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Alimonies_FamilyCourts_CourtId",
                table: "Alimonies",
                column: "CourtId",
                principalTable: "FamilyCourts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitationSchedules_FamilyCourts_CourtId",
                table: "VisitationSchedules",
                column: "CourtId",
                principalTable: "FamilyCourts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alimonies_FamilyCourts_CourtId",
                table: "Alimonies");

            migrationBuilder.DropForeignKey(
                name: "FK_VisitationSchedules_FamilyCourts_CourtId",
                table: "VisitationSchedules");

            migrationBuilder.DropIndex(
                name: "IX_VisitationSchedules_CourtId",
                table: "VisitationSchedules");

            migrationBuilder.DropIndex(
                name: "IX_Alimonies_CourtId",
                table: "Alimonies");
        }
    }
}
