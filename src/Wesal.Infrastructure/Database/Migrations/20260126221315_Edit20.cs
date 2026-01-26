using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wesal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Edit20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Custodies_Users_CustodianId",
                table: "Custodies");

            migrationBuilder.AddForeignKey(
                name: "FK_Custodies_Parents_CustodianId",
                table: "Custodies",
                column: "CustodianId",
                principalTable: "Parents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Custodies_Parents_CustodianId",
                table: "Custodies");

            migrationBuilder.AddForeignKey(
                name: "FK_Custodies_Users_CustodianId",
                table: "Custodies",
                column: "CustodianId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
