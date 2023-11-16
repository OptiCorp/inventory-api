using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inventoryapi.Migrations
{
    /// <inheritdoc />
    public partial class ModelConfigs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assemblies_Units_UnitId1",
                table: "Assemblies");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Subassemblies_SubassemblyId1",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Subassemblies_Assemblies_AssemblyId1",
                table: "Subassemblies");

            migrationBuilder.DropForeignKey(
                name: "FK_Subassemblies_Subassemblies_SubassemblyId",
                table: "Subassemblies");

            migrationBuilder.DropIndex(
                name: "IX_Subassemblies_AssemblyId1",
                table: "Subassemblies");

            migrationBuilder.DropIndex(
                name: "IX_Subassemblies_SubassemblyId",
                table: "Subassemblies");

            migrationBuilder.DropIndex(
                name: "IX_Items_SubassemblyId1",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Assemblies_UnitId1",
                table: "Assemblies");

            migrationBuilder.DropColumn(
                name: "AssemblyId1",
                table: "Subassemblies");

            migrationBuilder.DropColumn(
                name: "SubassemblyId",
                table: "Subassemblies");

            migrationBuilder.DropColumn(
                name: "SubassemblyId1",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "UnitId1",
                table: "Assemblies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssemblyId1",
                table: "Subassemblies",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubassemblyId",
                table: "Subassemblies",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubassemblyId1",
                table: "Items",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnitId1",
                table: "Assemblies",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subassemblies_AssemblyId1",
                table: "Subassemblies",
                column: "AssemblyId1");

            migrationBuilder.CreateIndex(
                name: "IX_Subassemblies_SubassemblyId",
                table: "Subassemblies",
                column: "SubassemblyId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_SubassemblyId1",
                table: "Items",
                column: "SubassemblyId1");

            migrationBuilder.CreateIndex(
                name: "IX_Assemblies_UnitId1",
                table: "Assemblies",
                column: "UnitId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Assemblies_Units_UnitId1",
                table: "Assemblies",
                column: "UnitId1",
                principalTable: "Units",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Subassemblies_SubassemblyId1",
                table: "Items",
                column: "SubassemblyId1",
                principalTable: "Subassemblies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subassemblies_Assemblies_AssemblyId1",
                table: "Subassemblies",
                column: "AssemblyId1",
                principalTable: "Assemblies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subassemblies_Subassemblies_SubassemblyId",
                table: "Subassemblies",
                column: "SubassemblyId",
                principalTable: "Subassemblies",
                principalColumn: "Id");
        }
    }
}
