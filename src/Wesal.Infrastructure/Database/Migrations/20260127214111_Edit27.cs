using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wesal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Edit27 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartDayInMonth",
                table: "VisitationSchedules");

            migrationBuilder.AddColumn<DateOnly>(
                name: "EndDate",
                table: "VisitationSchedules",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "StartDate",
                table: "VisitationSchedules",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "VisitationSchedules");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "VisitationSchedules");

            migrationBuilder.AddColumn<int>(
                name: "StartDayInMonth",
                table: "VisitationSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
