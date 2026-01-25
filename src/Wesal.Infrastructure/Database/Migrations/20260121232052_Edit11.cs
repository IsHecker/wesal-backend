using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wesal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Edit11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SchoolReports_ChildId",
                table: "SchoolReports",
                column: "ChildId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SchoolReports_SchoolId",
                table: "SchoolReports",
                column: "SchoolId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolReports_Children_ChildId",
                table: "SchoolReports",
                column: "ChildId",
                principalTable: "Children",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolReports_Schools_SchoolId",
                table: "SchoolReports",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolReports_Children_ChildId",
                table: "SchoolReports");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolReports_Schools_SchoolId",
                table: "SchoolReports");

            migrationBuilder.DropIndex(
                name: "IX_SchoolReports_ChildId",
                table: "SchoolReports");

            migrationBuilder.DropIndex(
                name: "IX_SchoolReports_SchoolId",
                table: "SchoolReports");
        }
    }
}
