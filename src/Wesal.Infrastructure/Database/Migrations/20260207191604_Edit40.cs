using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wesal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Edit40 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Visitations_CourtStaffs_VerifiedById",
                table: "Visitations");

            migrationBuilder.AddForeignKey(
                name: "FK_Visitations_VisitCenterStaffs_VerifiedById",
                table: "Visitations",
                column: "VerifiedById",
                principalTable: "VisitCenterStaffs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Visitations_VisitCenterStaffs_VerifiedById",
                table: "Visitations");

            migrationBuilder.AddForeignKey(
                name: "FK_Visitations_CourtStaffs_VerifiedById",
                table: "Visitations",
                column: "VerifiedById",
                principalTable: "CourtStaffs",
                principalColumn: "Id");
        }
    }
}
