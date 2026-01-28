using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wesal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Edit25 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalAlimonyPaymentsOverdue",
                table: "ComplianceMetrics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalVisitationsMissed",
                table: "ComplianceMetrics",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAlimonyPaymentsOverdue",
                table: "ComplianceMetrics");

            migrationBuilder.DropColumn(
                name: "TotalVisitationsMissed",
                table: "ComplianceMetrics");
        }
    }
}
