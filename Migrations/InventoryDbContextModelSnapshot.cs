﻿// <auto-generated />
using System;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace inventoryapi.Migrations
{
    [DbContext(typeof(InventoryDbContext))]
    partial class InventoryDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Inventory.Models.Assembly", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DocumentationId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UnitId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Vendor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WPId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Description");

                    b.HasIndex("SerialNumber");

                    b.HasIndex("UnitId");

                    b.HasIndex("UserId");

                    b.HasIndex("WPId");

                    b.ToTable("Assemblies");
                });

            modelBuilder.Entity("Inventory.Models.Item", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DocumentationId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ListId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParentId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProductNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Vendor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WpId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Description");

                    b.HasIndex("ListId");

                    b.HasIndex("ParentId");

                    b.HasIndex("SerialNumber");

                    b.HasIndex("UserId");

                    b.HasIndex("WpId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Inventory.Models.List", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("CreatedDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Lists");
                });

            modelBuilder.Entity("Inventory.Models.Part", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DocumentationId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SubassemblyId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Vendor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WPId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Description");

                    b.HasIndex("SerialNumber");

                    b.HasIndex("SubassemblyId");

                    b.HasIndex("UserId");

                    b.HasIndex("WPId");

                    b.ToTable("Parts");
                });

            modelBuilder.Entity("Inventory.Models.Subassembly", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AssemblyId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DocumentationId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParentSubassemblyId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProductNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Vendor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WPId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("AssemblyId");

                    b.HasIndex("Description");

                    b.HasIndex("ParentSubassemblyId");

                    b.HasIndex("SerialNumber");

                    b.HasIndex("UserId");

                    b.HasIndex("WPId");

                    b.ToTable("Subassemblies");
                });

            modelBuilder.Entity("Inventory.Models.Unit", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DocumentationId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Vendor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WPId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Description");

                    b.HasIndex("SerialNumber");

                    b.HasIndex("UserId");

                    b.HasIndex("WPId");

                    b.ToTable("Units");
                });

            modelBuilder.Entity("Inventory.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AzureAdUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("UmId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserRole")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Inventory.Models.Assembly", b =>
                {
                    b.HasOne("Inventory.Models.Unit", "Unit")
                        .WithMany("Assemblies")
                        .HasForeignKey("UnitId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Inventory.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Unit");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Inventory.Models.Item", b =>
                {
                    b.HasOne("Inventory.Models.List", null)
                        .WithMany("Items")
                        .HasForeignKey("ListId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Inventory.Models.Item", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Inventory.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Parent");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Inventory.Models.List", b =>
                {
                    b.HasOne("Inventory.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Inventory.Models.Part", b =>
                {
                    b.HasOne("Inventory.Models.Subassembly", "Subassembly")
                        .WithMany("Parts")
                        .HasForeignKey("SubassemblyId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Inventory.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Subassembly");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Inventory.Models.Subassembly", b =>
                {
                    b.HasOne("Inventory.Models.Assembly", "Assembly")
                        .WithMany("Subassemblies")
                        .HasForeignKey("AssemblyId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Inventory.Models.Subassembly", "ParentSubassembly")
                        .WithMany("Subassemblies")
                        .HasForeignKey("ParentSubassemblyId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Inventory.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Assembly");

                    b.Navigation("ParentSubassembly");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Inventory.Models.Unit", b =>
                {
                    b.HasOne("Inventory.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("User");
                });

            modelBuilder.Entity("Inventory.Models.Assembly", b =>
                {
                    b.Navigation("Subassemblies");
                });

            modelBuilder.Entity("Inventory.Models.Item", b =>
                {
                    b.Navigation("Children");
                });

            modelBuilder.Entity("Inventory.Models.List", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Inventory.Models.Subassembly", b =>
                {
                    b.Navigation("Parts");

                    b.Navigation("Subassemblies");
                });

            modelBuilder.Entity("Inventory.Models.Unit", b =>
                {
                    b.Navigation("Assemblies");
                });
#pragma warning restore 612, 618
        }
    }
}
