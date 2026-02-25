using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wesal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Edit49 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Alimonies_CourtId",
                table: "Alimonies");

            migrationBuilder.CreateIndex(
                name: "IX_Alimonies_CourtId",
                table: "Alimonies",
                column: "CourtId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Alimonies_CourtId",
                table: "Alimonies");

            migrationBuilder.CreateIndex(
                name: "IX_Alimonies_CourtId",
                table: "Alimonies",
                column: "CourtId",
                unique: true);
        }
    }
}
