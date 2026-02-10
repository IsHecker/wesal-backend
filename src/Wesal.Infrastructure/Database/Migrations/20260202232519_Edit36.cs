using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wesal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Edit36 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsTemporaryPassword",
                table: "Users",
                newName: "PasswordChangeRequired");

            migrationBuilder.AddColumn<Guid>(
                name: "CourtId",
                table: "VisitCenterStaffs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourtId",
                table: "VisitCenterStaffs");

            migrationBuilder.RenameColumn(
                name: "PasswordChangeRequired",
                table: "Users",
                newName: "IsTemporaryPassword");
        }
    }
}
