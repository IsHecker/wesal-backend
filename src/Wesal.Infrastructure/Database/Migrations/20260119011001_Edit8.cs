using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wesal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Edit8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DueDay",
                table: "Alimonies",
                newName: "StartDayInMonth");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DueDate",
                table: "PaymentsDue",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateOnly>(
                name: "LastGeneratedDate",
                table: "Alimonies",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastGeneratedDate",
                table: "Alimonies");

            migrationBuilder.RenameColumn(
                name: "StartDayInMonth",
                table: "Alimonies",
                newName: "DueDay");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DueDate",
                table: "PaymentsDue",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }
    }
}
