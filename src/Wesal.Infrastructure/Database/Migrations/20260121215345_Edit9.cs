using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wesal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Edit9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "VisitCenterStaffs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_VisitCenterStaffs_Email",
                table: "VisitCenterStaffs",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VisitCenterStaffs_LocationId",
                table: "VisitCenterStaffs",
                column: "LocationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VisitCenterStaffs_UserId",
                table: "VisitCenterStaffs",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VisitCenterStaffs_Users_UserId",
                table: "VisitCenterStaffs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitCenterStaffs_VisitationLocations_LocationId",
                table: "VisitCenterStaffs",
                column: "LocationId",
                principalTable: "VisitationLocations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VisitCenterStaffs_Users_UserId",
                table: "VisitCenterStaffs");

            migrationBuilder.DropForeignKey(
                name: "FK_VisitCenterStaffs_VisitationLocations_LocationId",
                table: "VisitCenterStaffs");

            migrationBuilder.DropIndex(
                name: "IX_VisitCenterStaffs_Email",
                table: "VisitCenterStaffs");

            migrationBuilder.DropIndex(
                name: "IX_VisitCenterStaffs_LocationId",
                table: "VisitCenterStaffs");

            migrationBuilder.DropIndex(
                name: "IX_VisitCenterStaffs_UserId",
                table: "VisitCenterStaffs");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "VisitCenterStaffs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
