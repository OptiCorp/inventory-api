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

            modelBuilder.Entity("DocumentItem", b =>
                {
                    b.Property<string>("DocumentsId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ItemId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("DocumentsId", "ItemId");

                    b.HasIndex("ItemId");

                    b.ToTable("DocumentItem", (string)null);
                });

            modelBuilder.Entity("DocumentItemTemplate", b =>
                {
                    b.Property<string>("DocumentsId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ItemTemplateId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("DocumentsId", "ItemTemplateId");

                    b.HasIndex("ItemTemplateId");

                    b.ToTable("DocumentItemTemplate", (string)null);
                });

            modelBuilder.Entity("Inventory.Models.Category", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Categories", (string)null);
                });

            modelBuilder.Entity("Inventory.Models.Document", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BlobId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContentType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocumentTypeId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("DocumentTypeId");

                    b.ToTable("Documents", (string)null);
                });

            modelBuilder.Entity("Inventory.Models.DocumentType", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DocumentTypes", (string)null);
                });

            modelBuilder.Entity("Inventory.Models.Item", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedById")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ItemTemplateId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ListId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LocationId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ParentId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PreCheckId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SerialNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("VendorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("WpId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("ItemTemplateId");

                    b.HasIndex("LocationId");

                    b.HasIndex("ParentId");

                    b.HasIndex("PreCheckId");

                    b.HasIndex("SerialNumber");

                    b.HasIndex("VendorId");

                    b.HasIndex("WpId");

                    b.ToTable("Items", (string)null);
                });

            modelBuilder.Entity("Inventory.Models.ItemTemplate", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CategoryId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedById")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Revision")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CreatedById");

                    b.ToTable("ItemTemplates", (string)null);
                });

            modelBuilder.Entity("Inventory.Models.List", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedById")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("Title");

                    b.ToTable("Lists", (string)null);
                });

            modelBuilder.Entity("Inventory.Models.Location", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Locations", (string)null);
                });

            modelBuilder.Entity("Inventory.Models.LogEntry", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedById")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ItemId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("ItemId");

                    b.ToTable("LogEntries", (string)null);
                });

            modelBuilder.Entity("Inventory.Models.PreCheck", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool?>("Check")
                        .HasColumnType("bit");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PreChecks", (string)null);
                });

            modelBuilder.Entity("Inventory.Models.Size", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<float?>("Amount")
                        .HasColumnType("real");

                    b.Property<string>("ItemTemplateId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Property")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Unit")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ItemTemplateId");

                    b.ToTable("Sizes", (string)null);
                });

            modelBuilder.Entity("Inventory.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AzureAdUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FirstName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("UmId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserRole")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Username")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("Inventory.Models.Vendor", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Vendors", (string)null);
                });

            modelBuilder.Entity("ItemList", b =>
                {
                    b.Property<string>("ItemsId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ListId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ItemsId", "ListId");

                    b.HasIndex("ListId");

                    b.ToTable("ItemList", (string)null);
                });

            modelBuilder.Entity("DocumentItem", b =>
                {
                    b.HasOne("Inventory.Models.Document", null)
                        .WithMany()
                        .HasForeignKey("DocumentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Inventory.Models.Item", null)
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DocumentItemTemplate", b =>
                {
                    b.HasOne("Inventory.Models.Document", null)
                        .WithMany()
                        .HasForeignKey("DocumentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Inventory.Models.ItemTemplate", null)
                        .WithMany()
                        .HasForeignKey("ItemTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Inventory.Models.Document", b =>
                {
                    b.HasOne("Inventory.Models.DocumentType", "DocumentType")
                        .WithMany()
                        .HasForeignKey("DocumentTypeId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("DocumentType");
                });

            modelBuilder.Entity("Inventory.Models.Item", b =>
                {
                    b.HasOne("Inventory.Models.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Inventory.Models.ItemTemplate", "ItemTemplate")
                        .WithMany()
                        .HasForeignKey("ItemTemplateId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Inventory.Models.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Inventory.Models.Item", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Inventory.Models.PreCheck", "PreCheck")
                        .WithMany()
                        .HasForeignKey("PreCheckId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Inventory.Models.Vendor", "Vendor")
                        .WithMany()
                        .HasForeignKey("VendorId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("CreatedBy");

                    b.Navigation("ItemTemplate");

                    b.Navigation("Location");

                    b.Navigation("Parent");

                    b.Navigation("PreCheck");

                    b.Navigation("Vendor");
                });

            modelBuilder.Entity("Inventory.Models.ItemTemplate", b =>
                {
                    b.HasOne("Inventory.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Inventory.Models.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.Navigation("Category");

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("Inventory.Models.List", b =>
                {
                    b.HasOne("Inventory.Models.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("Inventory.Models.LogEntry", b =>
                {
                    b.HasOne("Inventory.Models.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("Inventory.Models.Item", null)
                        .WithMany("LogEntries")
                        .HasForeignKey("ItemId");

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("Inventory.Models.Size", b =>
                {
                    b.HasOne("Inventory.Models.ItemTemplate", null)
                        .WithMany("Sizes")
                        .HasForeignKey("ItemTemplateId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("ItemList", b =>
                {
                    b.HasOne("Inventory.Models.Item", null)
                        .WithMany()
                        .HasForeignKey("ItemsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Inventory.Models.List", null)
                        .WithMany()
                        .HasForeignKey("ListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Inventory.Models.Item", b =>
                {
                    b.Navigation("Children");

                    b.Navigation("LogEntries");
                });

            modelBuilder.Entity("Inventory.Models.ItemTemplate", b =>
                {
                    b.Navigation("Sizes");
                });
#pragma warning restore 612, 618
        }
    }
}
