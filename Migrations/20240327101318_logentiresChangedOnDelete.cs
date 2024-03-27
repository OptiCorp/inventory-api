using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inventoryapi.Migrations
{
    /// <inheritdoc />
    public partial class logentiresChangedOnDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogEntries_ItemTemplates_ItemTemplateId",
                table: "LogEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_LogEntries_Items_ItemId",
                table: "LogEntries");

            migrationBuilder.AddForeignKey(
                name: "FK_LogEntries_ItemTemplates_ItemTemplateId",
                table: "LogEntries",
                column: "ItemTemplateId",
                principalTable: "ItemTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LogEntries_Items_ItemId",
                table: "LogEntries",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogEntries_ItemTemplates_ItemTemplateId",
                table: "LogEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_LogEntries_Items_ItemId",
                table: "LogEntries");

            migrationBuilder.AddForeignKey(
                name: "FK_LogEntries_ItemTemplates_ItemTemplateId",
                table: "LogEntries",
                column: "ItemTemplateId",
                principalTable: "ItemTemplates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LogEntries_Items_ItemId",
                table: "LogEntries",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");
        }
    }
}
