using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inventoryapi.Migrations
{
    /// <inheritdoc />
    public partial class VendorFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Vendors");
        }
    }
}
