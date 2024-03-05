using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inventoryapi.Migrations
{
    /// <inheritdoc />
    public partial class filenametodocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Documents");
        }
    }
}
