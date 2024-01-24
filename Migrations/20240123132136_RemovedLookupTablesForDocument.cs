using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inventoryapi.Migrations
{
    /// <inheritdoc />
    public partial class RemovedLookupTablesForDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentItem");

            migrationBuilder.DropTable(
                name: "DocumentItemTemplate");

            migrationBuilder.AddColumn<string>(
                name: "ItemId",
                table: "Documents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemTemplateId",
                table: "Documents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ItemId",
                table: "Documents",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ItemTemplateId",
                table: "Documents",
                column: "ItemTemplateId");

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

            migrationBuilder.DropIndex(
                name: "IX_Documents_ItemId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_ItemTemplateId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "ItemTemplateId",
                table: "Documents");

            migrationBuilder.CreateTable(
                name: "DocumentItem",
                columns: table => new
                {
                    DocumentsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentItem", x => new { x.DocumentsId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_DocumentItem_Documents_DocumentsId",
                        column: x => x.DocumentsId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentItem_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentItemTemplate",
                columns: table => new
                {
                    DocumentsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemTemplateId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentItemTemplate", x => new { x.DocumentsId, x.ItemTemplateId });
                    table.ForeignKey(
                        name: "FK_DocumentItemTemplate_Documents_DocumentsId",
                        column: x => x.DocumentsId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentItemTemplate_ItemTemplates_ItemTemplateId",
                        column: x => x.ItemTemplateId,
                        principalTable: "ItemTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentItem_ItemId",
                table: "DocumentItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentItemTemplate_ItemTemplateId",
                table: "DocumentItemTemplate",
                column: "ItemTemplateId");
        }
    }
}
