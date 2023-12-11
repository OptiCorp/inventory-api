using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inventoryapi.Migrations
{
    /// <inheritdoc />
    public partial class tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parts");

            migrationBuilder.DropTable(
                name: "Subassemblies");

            migrationBuilder.DropTable(
                name: "Assemblies");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Vendor",
                table: "Items");

            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "Items",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationId",
                table: "Items",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorId",
                table: "Items",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_CategoryId",
                table: "Items",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_LocationId",
                table: "Items",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_VendorId",
                table: "Items",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Categories_CategoryId",
                table: "Items",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Locations_LocationId",
                table: "Items",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Vendors_VendorId",
                table: "Items",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Categories_CategoryId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Locations_LocationId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Vendors_VendorId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_CategoryId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_LocationId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_VendorId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "Items");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vendor",
                table: "Items",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Vendor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WPId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Units_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Assemblies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UnitId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Vendor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WPId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assemblies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assemblies_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Assemblies_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Subassemblies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssemblyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ParentSubassemblyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Vendor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WPId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subassemblies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subassemblies_Assemblies_AssemblyId",
                        column: x => x.AssemblyId,
                        principalTable: "Assemblies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Subassemblies_Subassemblies_ParentSubassemblyId",
                        column: x => x.ParentSubassemblyId,
                        principalTable: "Subassemblies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Subassemblies_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Parts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubassemblyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Vendor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WPId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parts_Subassemblies_SubassemblyId",
                        column: x => x.SubassemblyId,
                        principalTable: "Subassemblies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Parts_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assemblies_Description",
                table: "Assemblies",
                column: "Description");

            migrationBuilder.CreateIndex(
                name: "IX_Assemblies_SerialNumber",
                table: "Assemblies",
                column: "SerialNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Assemblies_UnitId",
                table: "Assemblies",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Assemblies_UserId",
                table: "Assemblies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Assemblies_WPId",
                table: "Assemblies",
                column: "WPId");

            migrationBuilder.CreateIndex(
                name: "IX_Parts_Description",
                table: "Parts",
                column: "Description");

            migrationBuilder.CreateIndex(
                name: "IX_Parts_SerialNumber",
                table: "Parts",
                column: "SerialNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Parts_SubassemblyId",
                table: "Parts",
                column: "SubassemblyId");

            migrationBuilder.CreateIndex(
                name: "IX_Parts_UserId",
                table: "Parts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Parts_WPId",
                table: "Parts",
                column: "WPId");

            migrationBuilder.CreateIndex(
                name: "IX_Subassemblies_AssemblyId",
                table: "Subassemblies",
                column: "AssemblyId");

            migrationBuilder.CreateIndex(
                name: "IX_Subassemblies_Description",
                table: "Subassemblies",
                column: "Description");

            migrationBuilder.CreateIndex(
                name: "IX_Subassemblies_ParentSubassemblyId",
                table: "Subassemblies",
                column: "ParentSubassemblyId");

            migrationBuilder.CreateIndex(
                name: "IX_Subassemblies_SerialNumber",
                table: "Subassemblies",
                column: "SerialNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Subassemblies_UserId",
                table: "Subassemblies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Subassemblies_WPId",
                table: "Subassemblies",
                column: "WPId");

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
        }
    }
}
