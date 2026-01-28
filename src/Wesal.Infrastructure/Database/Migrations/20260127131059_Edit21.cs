using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wesal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Edit21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CustodyRequests_CourtCaseId",
                table: "CustodyRequests",
                column: "CourtCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CustodyRequests_CustodyId",
                table: "CustodyRequests",
                column: "CustodyId");

            migrationBuilder.CreateIndex(
                name: "IX_CustodyRequests_FamilyId",
                table: "CustodyRequests",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_CustodyRequests_ParentId",
                table: "CustodyRequests",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustodyRequests_CourtCases_CourtCaseId",
                table: "CustodyRequests",
                column: "CourtCaseId",
                principalTable: "CourtCases",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustodyRequests_Custodies_CustodyId",
                table: "CustodyRequests",
                column: "CustodyId",
                principalTable: "Custodies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustodyRequests_Families_FamilyId",
                table: "CustodyRequests",
                column: "FamilyId",
                principalTable: "Families",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustodyRequests_Parents_ParentId",
                table: "CustodyRequests",
                column: "ParentId",
                principalTable: "Parents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustodyRequests_CourtCases_CourtCaseId",
                table: "CustodyRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_CustodyRequests_Custodies_CustodyId",
                table: "CustodyRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_CustodyRequests_Families_FamilyId",
                table: "CustodyRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_CustodyRequests_Parents_ParentId",
                table: "CustodyRequests");

            migrationBuilder.DropIndex(
                name: "IX_CustodyRequests_CourtCaseId",
                table: "CustodyRequests");

            migrationBuilder.DropIndex(
                name: "IX_CustodyRequests_CustodyId",
                table: "CustodyRequests");

            migrationBuilder.DropIndex(
                name: "IX_CustodyRequests_FamilyId",
                table: "CustodyRequests");

            migrationBuilder.DropIndex(
                name: "IX_CustodyRequests_ParentId",
                table: "CustodyRequests");
        }
    }
}
