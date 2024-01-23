using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inventoryapi.Migrations
{
    /// <inheritdoc />
    public partial class MovedCategoryFromItemToTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Categories_CategoryId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_CategoryId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Items");

            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "ItemTemplates",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemTemplates_CategoryId",
                table: "ItemTemplates",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemTemplates_Categories_CategoryId",
                table: "ItemTemplates",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemTemplates_Categories_CategoryId",
                table: "ItemTemplates");

            migrationBuilder.DropIndex(
                name: "IX_ItemTemplates_CategoryId",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "ItemTemplates");

            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "Items",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_CategoryId",
                table: "Items",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Categories_CategoryId",
                table: "Items",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
