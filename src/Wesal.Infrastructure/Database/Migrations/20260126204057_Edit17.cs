using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wesal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Edit17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Complaints_ReporterId",
                table: "Complaints");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_ReporterId",
                table: "Complaints",
                column: "ReporterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Complaints_ReporterId",
                table: "Complaints");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_ReporterId",
                table: "Complaints",
                column: "ReporterId",
                unique: true);
        }
    }
}
