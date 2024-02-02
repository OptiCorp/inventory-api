using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inventoryapi.Migrations
{
    /// <inheritdoc />
    public partial class TemplateLogEntries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ItemTemplateId",
                table: "LogEntries",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LogEntries_ItemTemplateId",
                table: "LogEntries",
                column: "ItemTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_LogEntries_ItemTemplates_ItemTemplateId",
                table: "LogEntries",
                column: "ItemTemplateId",
                principalTable: "ItemTemplates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogEntries_ItemTemplates_ItemTemplateId",
                table: "LogEntries");

            migrationBuilder.DropIndex(
                name: "IX_LogEntries_ItemTemplateId",
                table: "LogEntries");

            migrationBuilder.DropColumn(
                name: "ItemTemplateId",
                table: "LogEntries");
        }
    }
}
