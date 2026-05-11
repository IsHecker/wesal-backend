using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wesal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Edit57 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CourtStaffs_CourtId",
                table: "CourtStaffs");

            migrationBuilder.CreateIndex(
                name: "IX_CourtStaffs_CourtId",
                table: "CourtStaffs",
                column: "CourtId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CourtStaffs_CourtId",
                table: "CourtStaffs");

            migrationBuilder.CreateIndex(
                name: "IX_CourtStaffs_CourtId",
                table: "CourtStaffs",
                column: "CourtId",
                unique: true);
        }
    }
}
