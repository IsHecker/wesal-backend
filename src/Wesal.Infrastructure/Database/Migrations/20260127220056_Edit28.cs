using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wesal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Edit28 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ComplianceMetrics_Date_FamilyId_ParentId",
                table: "ComplianceMetrics",
                columns: new[] { "Date", "FamilyId", "ParentId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ComplianceMetrics_Date_FamilyId_ParentId",
                table: "ComplianceMetrics");
        }
    }
}
