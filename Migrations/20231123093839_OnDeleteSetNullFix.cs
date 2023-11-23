using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inventoryapi.Migrations
{
    /// <inheritdoc />
    public partial class OnDeleteSetNullFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assemblies_User_UserId",
                table: "Assemblies");

            migrationBuilder.DropForeignKey(
                name: "FK_Parts_User_UserId",
                table: "Parts");

            migrationBuilder.DropForeignKey(
                name: "FK_Subassemblies_User_UserId",
                table: "Subassemblies");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Items",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_UserId",
                table: "Items",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assemblies_User_UserId",
                table: "Assemblies",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_User_UserId",
                table: "Items",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Parts_User_UserId",
                table: "Parts",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Subassemblies_User_UserId",
                table: "Subassemblies",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assemblies_User_UserId",
                table: "Assemblies");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_User_UserId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Parts_User_UserId",
                table: "Parts");

            migrationBuilder.DropForeignKey(
                name: "FK_Subassemblies_User_UserId",
                table: "Subassemblies");

            migrationBuilder.DropIndex(
                name: "IX_Items_UserId",
                table: "Items");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Assemblies_User_UserId",
                table: "Assemblies",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Parts_User_UserId",
                table: "Parts",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subassemblies_User_UserId",
                table: "Subassemblies",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
