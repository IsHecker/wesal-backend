using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wesal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Edit59 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VisitCenterStaffs_LocationId",
                table: "VisitCenterStaffs");

            migrationBuilder.CreateIndex(
                name: "IX_VisitCenterStaffs_LocationId",
                table: "VisitCenterStaffs",
                column: "LocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VisitCenterStaffs_LocationId",
                table: "VisitCenterStaffs");

            migrationBuilder.CreateIndex(
                name: "IX_VisitCenterStaffs_LocationId",
                table: "VisitCenterStaffs",
                column: "LocationId",
                unique: true);
        }
    }
}