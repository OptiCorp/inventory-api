using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inventoryapi.Migrations
{
    /// <inheritdoc />
    public partial class Id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assemblies_Units_UnitId",
                table: "Assemblies");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Subassemblies_SubassemblyId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Subassemblies_Assemblies_AssemblyId",
                table: "Subassemblies");

            migrationBuilder.DropForeignKey(
                name: "FK_Subassemblies_Subassemblies_SubassemblyId",
                table: "Subassemblies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Units",
                table: "Units");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subassemblies",
                table: "Subassemblies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Items",
                table: "Items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Assemblies",
                table: "Assemblies");

            migrationBuilder.AlterColumn<string>(
                name: "WPId",
                table: "Units",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Units",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "WPId",
                table: "Subassemblies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Subassemblies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "WPId",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Items",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "WPId",
                table: "Assemblies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Assemblies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Units",
                table: "Units",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subassemblies",
                table: "Subassemblies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Items",
                table: "Items",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assemblies",
                table: "Assemblies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Assemblies_Units_UnitId",
                table: "Assemblies",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Subassemblies_SubassemblyId",
                table: "Items",
                column: "SubassemblyId",
                principalTable: "Subassemblies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subassemblies_Assemblies_AssemblyId",
                table: "Subassemblies",
                column: "AssemblyId",
                principalTable: "Assemblies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subassemblies_Subassemblies_SubassemblyId",
                table: "Subassemblies",
                column: "SubassemblyId",
                principalTable: "Subassemblies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assemblies_Units_UnitId",
                table: "Assemblies");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Subassemblies_SubassemblyId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Subassemblies_Assemblies_AssemblyId",
                table: "Subassemblies");

            migrationBuilder.DropForeignKey(
                name: "FK_Subassemblies_Subassemblies_SubassemblyId",
                table: "Subassemblies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Units",
                table: "Units");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subassemblies",
                table: "Subassemblies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Items",
                table: "Items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Assemblies",
                table: "Assemblies");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Subassemblies");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Assemblies");

            migrationBuilder.AlterColumn<string>(
                name: "WPId",
                table: "Units",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WPId",
                table: "Subassemblies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WPId",
                table: "Items",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WPId",
                table: "Assemblies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Units",
                table: "Units",
                column: "WPId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subassemblies",
                table: "Subassemblies",
                column: "WPId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Items",
                table: "Items",
                column: "WPId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assemblies",
                table: "Assemblies",
                column: "WPId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assemblies_Units_UnitId",
                table: "Assemblies",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "WPId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Subassemblies_SubassemblyId",
                table: "Items",
                column: "SubassemblyId",
                principalTable: "Subassemblies",
                principalColumn: "WPId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subassemblies_Assemblies_AssemblyId",
                table: "Subassemblies",
                column: "AssemblyId",
                principalTable: "Assemblies",
                principalColumn: "WPId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subassemblies_Subassemblies_SubassemblyId",
                table: "Subassemblies",
                column: "SubassemblyId",
                principalTable: "Subassemblies",
                principalColumn: "WPId");
        }
    }
}
