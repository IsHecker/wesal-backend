using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wesal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RedesignCustodyRequests_Point7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProcessedAt",
                table: "CustodyRequests",
                newName: "RespondedAt");

            migrationBuilder.RenameColumn(
                name: "DecisionNote",
                table: "CustodyRequests",
                newName: "ReasonNote");

            migrationBuilder.AddColumn<Guid>(
                name: "CustodialParentId",
                table: "CustodyRequests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CustodyRequests_CustodialParentId",
                table: "CustodyRequests",
                column: "CustodialParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustodyRequests_Parents_CustodialParentId",
                table: "CustodyRequests",
                column: "CustodialParentId",
                principalTable: "Parents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustodyRequests_Parents_CustodialParentId",
                table: "CustodyRequests");

            migrationBuilder.DropIndex(
                name: "IX_CustodyRequests_CustodialParentId",
                table: "CustodyRequests");

            migrationBuilder.DropColumn(
                name: "CustodialParentId",
                table: "CustodyRequests");

            migrationBuilder.RenameColumn(
                name: "RespondedAt",
                table: "CustodyRequests",
                newName: "ProcessedAt");

            migrationBuilder.RenameColumn(
                name: "ReasonNote",
                table: "CustodyRequests",
                newName: "DecisionNote");
        }
    }
}
