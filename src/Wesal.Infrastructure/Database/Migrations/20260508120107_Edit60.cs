using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wesal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Edit60 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResolutionNotes",
                table: "Complaints",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "CompletedAt",
                table: "Complaints",
                newName: "ResolvedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "RejectedAt",
                table: "Complaints",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RejectedAt",
                table: "Complaints");

            migrationBuilder.RenameColumn(
                name: "ResolvedAt",
                table: "Complaints",
                newName: "CompletedAt");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "Complaints",
                newName: "ResolutionNotes");
        }
    }
}
