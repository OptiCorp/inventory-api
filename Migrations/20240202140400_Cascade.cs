using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inventoryapi.Migrations
{
    /// <inheritdoc />
    public partial class Cascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_ItemTemplates_ItemTemplateId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Items_ItemId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Sizes_ItemTemplates_ItemTemplateId",
                table: "Sizes");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_ItemTemplates_ItemTemplateId",
                table: "Documents",
                column: "ItemTemplateId",
                principalTable: "ItemTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Items_ItemId",
                table: "Documents",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sizes_ItemTemplates_ItemTemplateId",
                table: "Sizes",
                column: "ItemTemplateId",
                principalTable: "ItemTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_ItemTemplates_ItemTemplateId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Items_ItemId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Sizes_ItemTemplates_ItemTemplateId",
                table: "Sizes");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_ItemTemplates_ItemTemplateId",
                table: "Documents",
                column: "ItemTemplateId",
                principalTable: "ItemTemplates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Items_ItemId",
                table: "Documents",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sizes_ItemTemplates_ItemTemplateId",
                table: "Sizes",
                column: "ItemTemplateId",
                principalTable: "ItemTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
