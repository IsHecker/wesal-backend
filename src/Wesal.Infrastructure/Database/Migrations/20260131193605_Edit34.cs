using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wesal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Edit34 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notifications_RecipientId_RelatedEntityId_Type",
                table: "Notifications");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "EntityType",
                table: "Notifications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_RecipientId_RelatedEntityId_EntityType",
                table: "Notifications",
                columns: new[] { "RecipientId", "RelatedEntityId", "EntityType" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notifications_RecipientId_RelatedEntityId_EntityType",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "Notifications");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Notifications",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_RecipientId_RelatedEntityId_Type",
                table: "Notifications",
                columns: new[] { "RecipientId", "RelatedEntityId", "Type" });
        }
    }
}
