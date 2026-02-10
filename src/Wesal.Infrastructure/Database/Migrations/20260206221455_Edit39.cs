using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wesal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Edit39 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Custodies_Parents_CustodianId",
                table: "Custodies");

            migrationBuilder.DropForeignKey(
                name: "FK_Visitations_Parents_ParentId",
                table: "Visitations");

            migrationBuilder.DropForeignKey(
                name: "FK_VisitationSchedules_Parents_ParentId",
                table: "VisitationSchedules");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "VisitationSchedules",
                newName: "NonCustodialParentId");

            migrationBuilder.RenameIndex(
                name: "IX_VisitationSchedules_ParentId",
                table: "VisitationSchedules",
                newName: "IX_VisitationSchedules_NonCustodialParentId");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "Visitations",
                newName: "NonCustodialParentId");

            migrationBuilder.RenameColumn(
                name: "CheckedInAt",
                table: "Visitations",
                newName: "NonCustodialCheckedInAt");

            migrationBuilder.RenameIndex(
                name: "IX_Visitations_ParentId",
                table: "Visitations",
                newName: "IX_Visitations_NonCustodialParentId");

            migrationBuilder.RenameColumn(
                name: "CustodianId",
                table: "Custodies",
                newName: "NonCustodialParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Custodies_CustodianId",
                table: "Custodies",
                newName: "IX_Custodies_NonCustodialParentId");

            migrationBuilder.AddColumn<string>(
                name: "CustodialNationalId",
                table: "VisitationSchedules",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "CustodialParentId",
                table: "VisitationSchedules",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "NonCustodialNationalId",
                table: "VisitationSchedules",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompanionCheckedInAt",
                table: "Visitations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanionNationalId",
                table: "Visitations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NonCustodialNationalId",
                table: "Visitations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsWithdrawn",
                table: "Payments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "WithdrawalStatus",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WithdrawnAt",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFather",
                table: "Parents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "CustodialParentId",
                table: "Custodies",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_VisitationSchedules_CustodialParentId",
                table: "VisitationSchedules",
                column: "CustodialParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Custodies_CustodialParentId",
                table: "Custodies",
                column: "CustodialParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Custodies_Parents_CustodialParentId",
                table: "Custodies",
                column: "CustodialParentId",
                principalTable: "Parents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Custodies_Parents_NonCustodialParentId",
                table: "Custodies",
                column: "NonCustodialParentId",
                principalTable: "Parents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Visitations_Parents_NonCustodialParentId",
                table: "Visitations",
                column: "NonCustodialParentId",
                principalTable: "Parents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitationSchedules_Parents_CustodialParentId",
                table: "VisitationSchedules",
                column: "CustodialParentId",
                principalTable: "Parents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitationSchedules_Parents_NonCustodialParentId",
                table: "VisitationSchedules",
                column: "NonCustodialParentId",
                principalTable: "Parents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Custodies_Parents_CustodialParentId",
                table: "Custodies");

            migrationBuilder.DropForeignKey(
                name: "FK_Custodies_Parents_NonCustodialParentId",
                table: "Custodies");

            migrationBuilder.DropForeignKey(
                name: "FK_Visitations_Parents_NonCustodialParentId",
                table: "Visitations");

            migrationBuilder.DropForeignKey(
                name: "FK_VisitationSchedules_Parents_CustodialParentId",
                table: "VisitationSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_VisitationSchedules_Parents_NonCustodialParentId",
                table: "VisitationSchedules");

            migrationBuilder.DropIndex(
                name: "IX_VisitationSchedules_CustodialParentId",
                table: "VisitationSchedules");

            migrationBuilder.DropIndex(
                name: "IX_Custodies_CustodialParentId",
                table: "Custodies");

            migrationBuilder.DropColumn(
                name: "CustodialNationalId",
                table: "VisitationSchedules");

            migrationBuilder.DropColumn(
                name: "CustodialParentId",
                table: "VisitationSchedules");

            migrationBuilder.DropColumn(
                name: "NonCustodialNationalId",
                table: "VisitationSchedules");

            migrationBuilder.DropColumn(
                name: "CompanionCheckedInAt",
                table: "Visitations");

            migrationBuilder.DropColumn(
                name: "CompanionNationalId",
                table: "Visitations");

            migrationBuilder.DropColumn(
                name: "NonCustodialNationalId",
                table: "Visitations");

            migrationBuilder.DropColumn(
                name: "IsWithdrawn",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "WithdrawalStatus",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "WithdrawnAt",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsFather",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "CustodialParentId",
                table: "Custodies");

            migrationBuilder.RenameColumn(
                name: "NonCustodialParentId",
                table: "VisitationSchedules",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_VisitationSchedules_NonCustodialParentId",
                table: "VisitationSchedules",
                newName: "IX_VisitationSchedules_ParentId");

            migrationBuilder.RenameColumn(
                name: "NonCustodialParentId",
                table: "Visitations",
                newName: "ParentId");

            migrationBuilder.RenameColumn(
                name: "NonCustodialCheckedInAt",
                table: "Visitations",
                newName: "CheckedInAt");

            migrationBuilder.RenameIndex(
                name: "IX_Visitations_NonCustodialParentId",
                table: "Visitations",
                newName: "IX_Visitations_ParentId");

            migrationBuilder.RenameColumn(
                name: "NonCustodialParentId",
                table: "Custodies",
                newName: "CustodianId");

            migrationBuilder.RenameIndex(
                name: "IX_Custodies_NonCustodialParentId",
                table: "Custodies",
                newName: "IX_Custodies_CustodianId");

            migrationBuilder.AddForeignKey(
                name: "FK_Custodies_Parents_CustodianId",
                table: "Custodies",
                column: "CustodianId",
                principalTable: "Parents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Visitations_Parents_ParentId",
                table: "Visitations",
                column: "ParentId",
                principalTable: "Parents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitationSchedules_Parents_ParentId",
                table: "VisitationSchedules",
                column: "ParentId",
                principalTable: "Parents",
                principalColumn: "Id");
        }
    }
}
