using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wesal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Edit55 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttendedChildrenIds",
                table: "Visitations");

            migrationBuilder.DropColumn(
                name: "CompanionCheckedInAt",
                table: "Visitations");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "Visitations");

            migrationBuilder.DropColumn(
                name: "NonCustodialCheckedInAt",
                table: "Visitations");

            migrationBuilder.AddColumn<string>(
                name: "Attendance",
                table: "Visitations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attendance",
                table: "Visitations");

            migrationBuilder.AddColumn<string>(
                name: "AttendedChildrenIds",
                table: "Visitations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompanionCheckedInAt",
                table: "Visitations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "Visitations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NonCustodialCheckedInAt",
                table: "Visitations",
                type: "datetime2",
                nullable: true);
        }
    }
}
