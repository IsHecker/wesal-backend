using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wesal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Edit56 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssignedStaffId",
                table: "Families",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentLoad",
                table: "CourtStaffs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CourtStaffs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "CourtStaffs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Families_AssignedStaffId",
                table: "Families",
                column: "AssignedStaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_Families_CourtStaffs_AssignedStaffId",
                table: "Families",
                column: "AssignedStaffId",
                principalTable: "CourtStaffs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Families_CourtStaffs_AssignedStaffId",
                table: "Families");

            migrationBuilder.DropIndex(
                name: "IX_Families_AssignedStaffId",
                table: "Families");

            migrationBuilder.DropColumn(
                name: "AssignedStaffId",
                table: "Families");

            migrationBuilder.DropColumn(
                name: "CurrentLoad",
                table: "CourtStaffs");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CourtStaffs");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "CourtStaffs");
        }
    }
}
