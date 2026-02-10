using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wesal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Edit44 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Alimonies_AlimonyId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_AlimonyId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "AlimonyId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsWithdrawn",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "WithdrawnAt",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "WithdrawalStatus",
                table: "Payments",
                newName: "PaymentIntentId");

            migrationBuilder.AddColumn<string>(
                name: "WithdrawalStatus",
                table: "PaymentsDue",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WithdrawnAt",
                table: "PaymentsDue",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReceiptUrl",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WithdrawalStatus",
                table: "PaymentsDue");

            migrationBuilder.DropColumn(
                name: "WithdrawnAt",
                table: "PaymentsDue");

            migrationBuilder.RenameColumn(
                name: "PaymentIntentId",
                table: "Payments",
                newName: "WithdrawalStatus");

            migrationBuilder.AlterColumn<string>(
                name: "ReceiptUrl",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AlimonyId",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<long>(
                name: "Amount",
                table: "Payments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsWithdrawn",
                table: "Payments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "WithdrawnAt",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_AlimonyId",
                table: "Payments",
                column: "AlimonyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Alimonies_AlimonyId",
                table: "Payments",
                column: "AlimonyId",
                principalTable: "Alimonies",
                principalColumn: "Id");
        }
    }
}
