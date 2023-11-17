﻿using Microsoft.EntityFrameworkCore;
using Inventory.Models;

namespace Inventory.Configuration
{
    public static class UserConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);
        }
    }

    public static class AssemblyConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Assembly>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Assembly>()
                .HasOne(c => c.Unit)
                .WithMany(c => c.Assemblies)
                .HasForeignKey(c => c.UnitId);

            modelBuilder.Entity<Assembly>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Assembly>()
                .HasIndex(c => c.WPId);

            modelBuilder.Entity<Assembly>()
                .HasIndex(c => c.SerialNumber);

            modelBuilder.Entity<Assembly>()
                .HasIndex(c => c.Description);
        }
    }

    public static class ItemConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Item>()
                .HasOne(c => c.Subassembly)
                .WithMany(c => c.Items)
                .HasForeignKey(c => c.SubassemblyId);

            modelBuilder.Entity<Item>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Item>()
                .HasIndex(c => c.WPId);

            modelBuilder.Entity<Item>()
                .HasIndex(c => c.SerialNumber);

            modelBuilder.Entity<Item>()
                .HasIndex(c => c.Description);
        }
    }

    public static class SubassemblyConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subassembly>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Subassembly>()
                .HasOne(c => c.ParentSubassembly)
                .WithMany(c => c.Subassemblies)
                .HasForeignKey(c => c.ParentSubassemblyId);

            modelBuilder.Entity<Subassembly>()
                .HasOne(c => c.Assembly)
                .WithMany(c => c.Subassemblies)
                .HasForeignKey(c => c.AssemblyId);

            modelBuilder.Entity<Subassembly>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Subassembly>()
                .HasIndex(c => c.WPId);

            modelBuilder.Entity<Subassembly>()
                .HasIndex(c => c.SerialNumber);

            modelBuilder.Entity<Subassembly>()
                .HasIndex(c => c.Description);
        }
    }

    public static class UnitConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Unit>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Unit>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Unit>()
                .HasIndex(c => c.WPId);

            modelBuilder.Entity<Unit>()
                .HasIndex(c => c.SerialNumber);

            modelBuilder.Entity<Unit>()
                .HasIndex(c => c.Description);
        }
    }
}
