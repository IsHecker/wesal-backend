using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wesal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Edit52 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SchoolReports_ChildId",
                table: "SchoolReports");

            migrationBuilder.DropIndex(
                name: "IX_SchoolReports_SchoolId",
                table: "SchoolReports");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolReports_ChildId",
                table: "SchoolReports",
                column: "ChildId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolReports_SchoolId",
                table: "SchoolReports",
                column: "SchoolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SchoolReports_ChildId",
                table: "SchoolReports");

            migrationBuilder.DropIndex(
                name: "IX_SchoolReports_SchoolId",
                table: "SchoolReports");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolReports_ChildId",
                table: "SchoolReports",
                column: "ChildId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SchoolReports_SchoolId",
                table: "SchoolReports",
                column: "SchoolId",
                unique: true);
        }
    }
}
