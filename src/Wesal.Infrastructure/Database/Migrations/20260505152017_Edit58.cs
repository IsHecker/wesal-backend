using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wesal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Edit58 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentLoad",
                table: "CourtStaffs");

            migrationBuilder.AddColumn<Guid>(
                name: "AssignedStaffId",
                table: "ObligationAlerts",
                type: "uniqueidentifier",
                nullable: true,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ResolvedAt",
                table: "Families",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Families",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "AssignedStaffId",
                table: "CourtCases",
                type: "uniqueidentifier",
                nullable: true,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AssignedStaffId",
                table: "Complaints",
                type: "uniqueidentifier",
                nullable: true,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "StaffWorkload",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourtStaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoadCount = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffWorkload", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffWorkload_CourtStaffs_CourtStaffId",
                        column: x => x.CourtStaffId,
                        principalTable: "CourtStaffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ObligationAlerts_AssignedStaffId",
                table: "ObligationAlerts",
                column: "AssignedStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_CourtCases_AssignedStaffId",
                table: "CourtCases",
                column: "AssignedStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_AssignedStaffId",
                table: "Complaints",
                column: "AssignedStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffWorkload_CourtStaffId_Type",
                table: "StaffWorkload",
                columns: new[] { "CourtStaffId", "Type" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_CourtStaffs_AssignedStaffId",
                table: "Complaints",
                column: "AssignedStaffId",
                principalTable: "CourtStaffs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourtCases_CourtStaffs_AssignedStaffId",
                table: "CourtCases",
                column: "AssignedStaffId",
                principalTable: "CourtStaffs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ObligationAlerts_CourtStaffs_AssignedStaffId",
                table: "ObligationAlerts",
                column: "AssignedStaffId",
                principalTable: "CourtStaffs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_CourtStaffs_AssignedStaffId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_CourtCases_CourtStaffs_AssignedStaffId",
                table: "CourtCases");

            migrationBuilder.DropForeignKey(
                name: "FK_ObligationAlerts_CourtStaffs_AssignedStaffId",
                table: "ObligationAlerts");

            migrationBuilder.DropTable(
                name: "StaffWorkload");

            migrationBuilder.DropIndex(
                name: "IX_ObligationAlerts_AssignedStaffId",
                table: "ObligationAlerts");

            migrationBuilder.DropIndex(
                name: "IX_CourtCases_AssignedStaffId",
                table: "CourtCases");

            migrationBuilder.DropIndex(
                name: "IX_Complaints_AssignedStaffId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "AssignedStaffId",
                table: "ObligationAlerts");

            migrationBuilder.DropColumn(
                name: "ResolvedAt",
                table: "Families");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Families");

            migrationBuilder.DropColumn(
                name: "AssignedStaffId",
                table: "CourtCases");

            migrationBuilder.DropColumn(
                name: "AssignedStaffId",
                table: "Complaints");

            migrationBuilder.AddColumn<int>(
                name: "CurrentLoad",
                table: "CourtStaffs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
