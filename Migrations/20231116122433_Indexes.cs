using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inventoryapi.Migrations
{
    /// <inheritdoc />
    public partial class Indexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WPId",
                table: "Units",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Units",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "Units",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Units",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "WPId",
                table: "Subassemblies",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "Subassemblies",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Subassemblies",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "WPId",
                table: "Items",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "Items",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Items",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "WPId",
                table: "Assemblies",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "Assemblies",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Assemblies",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Units_Description",
                table: "Units",
                column: "Description");

            migrationBuilder.CreateIndex(
                name: "IX_Units_SerialNumber",
                table: "Units",
                column: "SerialNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Units_UserId",
                table: "Units",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_WPId",
                table: "Units",
                column: "WPId");

            migrationBuilder.CreateIndex(
                name: "IX_Subassemblies_Description",
                table: "Subassemblies",
                column: "Description");

            migrationBuilder.CreateIndex(
                name: "IX_Subassemblies_SerialNumber",
                table: "Subassemblies",
                column: "SerialNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Subassemblies_WPId",
                table: "Subassemblies",
                column: "WPId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_Description",
                table: "Items",
                column: "Description");

            migrationBuilder.CreateIndex(
                name: "IX_Items_SerialNumber",
                table: "Items",
                column: "SerialNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Items_WPId",
                table: "Items",
                column: "WPId");

            migrationBuilder.CreateIndex(
                name: "IX_Assemblies_Description",
                table: "Assemblies",
                column: "Description");

            migrationBuilder.CreateIndex(
                name: "IX_Assemblies_SerialNumber",
                table: "Assemblies",
                column: "SerialNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Assemblies_WPId",
                table: "Assemblies",
                column: "WPId");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_User_UserId",
                table: "Units",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_User_UserId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_Description",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_SerialNumber",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_UserId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_WPId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Subassemblies_Description",
                table: "Subassemblies");

            migrationBuilder.DropIndex(
                name: "IX_Subassemblies_SerialNumber",
                table: "Subassemblies");

            migrationBuilder.DropIndex(
                name: "IX_Subassemblies_WPId",
                table: "Subassemblies");

            migrationBuilder.DropIndex(
                name: "IX_Items_Description",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_SerialNumber",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_WPId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Assemblies_Description",
                table: "Assemblies");

            migrationBuilder.DropIndex(
                name: "IX_Assemblies_SerialNumber",
                table: "Assemblies");

            migrationBuilder.DropIndex(
                name: "IX_Assemblies_WPId",
                table: "Assemblies");

            migrationBuilder.AlterColumn<string>(
                name: "WPId",
                table: "Units",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Units",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "Units",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Units",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "WPId",
                table: "Subassemblies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "Subassemblies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Subassemblies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "WPId",
                table: "Items",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "Items",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Items",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "WPId",
                table: "Assemblies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "Assemblies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Assemblies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
