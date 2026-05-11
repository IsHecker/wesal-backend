using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wesal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class UnifyComplaintCompletionFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RejectedAt",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Complaints");

            migrationBuilder.RenameColumn(
                name: "ResolvedAt",
                table: "Complaints",
                newName: "CompletedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CompletedAt",
                table: "Complaints",
                newName: "ResolvedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "RejectedAt",
                table: "Complaints",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
