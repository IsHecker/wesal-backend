using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wesal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Edit12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alimonies_Users_PayerId",
                table: "Alimonies");

            migrationBuilder.DropForeignKey(
                name: "FK_Alimonies_Users_RecipientId",
                table: "Alimonies");

            migrationBuilder.DropColumn(
                name: "EndAt",
                table: "Alimonies");

            migrationBuilder.DropColumn(
                name: "StartAt",
                table: "Alimonies");

            migrationBuilder.DropColumn(
                name: "StartDayInMonth",
                table: "Alimonies");

            migrationBuilder.AddColumn<DateOnly>(
                name: "EndDate",
                table: "Alimonies",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "StartDate",
                table: "Alimonies",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddForeignKey(
                name: "FK_Alimonies_Parents_PayerId",
                table: "Alimonies",
                column: "PayerId",
                principalTable: "Parents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Alimonies_Parents_RecipientId",
                table: "Alimonies",
                column: "RecipientId",
                principalTable: "Parents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alimonies_Parents_PayerId",
                table: "Alimonies");

            migrationBuilder.DropForeignKey(
                name: "FK_Alimonies_Parents_RecipientId",
                table: "Alimonies");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Alimonies");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Alimonies");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndAt",
                table: "Alimonies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartAt",
                table: "Alimonies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "StartDayInMonth",
                table: "Alimonies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Alimonies_Users_PayerId",
                table: "Alimonies",
                column: "PayerId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Alimonies_Users_RecipientId",
                table: "Alimonies",
                column: "RecipientId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
