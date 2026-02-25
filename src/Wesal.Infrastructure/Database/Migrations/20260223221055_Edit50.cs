using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wesal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Edit50 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustodyRequests_Parents_ParentId",
                table: "CustodyRequests");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "CustodyRequests",
                newName: "NonCustodialParentId");

            migrationBuilder.RenameIndex(
                name: "IX_CustodyRequests_ParentId",
                table: "CustodyRequests",
                newName: "IX_CustodyRequests_NonCustodialParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustodyRequests_Parents_NonCustodialParentId",
                table: "CustodyRequests",
                column: "NonCustodialParentId",
                principalTable: "Parents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustodyRequests_Parents_NonCustodialParentId",
                table: "CustodyRequests");

            migrationBuilder.RenameColumn(
                name: "NonCustodialParentId",
                table: "CustodyRequests",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_CustodyRequests_NonCustodialParentId",
                table: "CustodyRequests",
                newName: "IX_CustodyRequests_ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustodyRequests_Parents_ParentId",
                table: "CustodyRequests",
                column: "ParentId",
                principalTable: "Parents",
                principalColumn: "Id");
        }
    }
}
