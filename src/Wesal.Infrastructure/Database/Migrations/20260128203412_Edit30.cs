using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wesal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Edit30 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportUrl",
                table: "SchoolReports");

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentId",
                table: "SchoolReports",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentId",
                table: "CourtCases",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentId",
                table: "Complaints",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SchoolReports_DocumentId",
                table: "SchoolReports",
                column: "DocumentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourtCases_DocumentId",
                table: "CourtCases",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_DocumentId",
                table: "Complaints",
                column: "DocumentId",
                unique: true,
                filter: "[DocumentId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_Documents_DocumentId",
                table: "Complaints",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourtCases_Documents_DocumentId",
                table: "CourtCases",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolReports_Documents_DocumentId",
                table: "SchoolReports",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_Documents_DocumentId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_CourtCases_Documents_DocumentId",
                table: "CourtCases");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolReports_Documents_DocumentId",
                table: "SchoolReports");

            migrationBuilder.DropIndex(
                name: "IX_SchoolReports_DocumentId",
                table: "SchoolReports");

            migrationBuilder.DropIndex(
                name: "IX_CourtCases_DocumentId",
                table: "CourtCases");

            migrationBuilder.DropIndex(
                name: "IX_Complaints_DocumentId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "SchoolReports");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "CourtCases");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "Complaints");

            migrationBuilder.AddColumn<string>(
                name: "ReportUrl",
                table: "SchoolReports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
