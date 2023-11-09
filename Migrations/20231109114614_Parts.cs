using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inventoryapi.Migrations
{
    /// <inheritdoc />
    public partial class Parts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    WPId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.WPId);
                });

            migrationBuilder.CreateTable(
                name: "Assemblies",
                columns: table => new
                {
                    WPId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assemblies", x => x.WPId);
                    table.ForeignKey(
                        name: "FK_Assemblies_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "WPId");
                });

            migrationBuilder.CreateTable(
                name: "Subassemblies",
                columns: table => new
                {
                    WPId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssemblyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SubassemblyId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subassemblies", x => x.WPId);
                    table.ForeignKey(
                        name: "FK_Subassemblies_Assemblies_AssemblyId",
                        column: x => x.AssemblyId,
                        principalTable: "Assemblies",
                        principalColumn: "WPId");
                    table.ForeignKey(
                        name: "FK_Subassemblies_Subassemblies_SubassemblyId",
                        column: x => x.SubassemblyId,
                        principalTable: "Subassemblies",
                        principalColumn: "WPId");
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    WPId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubassemblyId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.WPId);
                    table.ForeignKey(
                        name: "FK_Items_Subassemblies_SubassemblyId",
                        column: x => x.SubassemblyId,
                        principalTable: "Subassemblies",
                        principalColumn: "WPId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assemblies_UnitId",
                table: "Assemblies",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_SubassemblyId",
                table: "Items",
                column: "SubassemblyId");

            migrationBuilder.CreateIndex(
                name: "IX_Subassemblies_AssemblyId",
                table: "Subassemblies",
                column: "AssemblyId");

            migrationBuilder.CreateIndex(
                name: "IX_Subassemblies_SubassemblyId",
                table: "Subassemblies",
                column: "SubassemblyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Subassemblies");

            migrationBuilder.DropTable(
                name: "Assemblies");

            migrationBuilder.DropTable(
                name: "Units");
        }
    }
}
