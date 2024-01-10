using Microsoft.EntityFrameworkCore;
using Inventory.Models;

namespace Inventory.Configuration
{
    public static class UserConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);
            
            modelBuilder.Entity<User>()
                .HasOne(c => c.UserRole)
                .WithMany()
                .HasForeignKey(c => c.UserRoleId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
    
    public static class UserRoleConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(u => u.Id);
        }
    }
    
    public static class ItemConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Item>()
                .HasOne(c => c.Parent)
                .WithMany(c => c.Children)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<Item>()
                .HasOne(c => c.CreatedBy)
                .WithMany()
                .HasForeignKey(c => c.CreatedById)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Item>()
                .HasIndex(c => c.WpId);

            modelBuilder.Entity<Item>()
                .HasIndex(c => c.SerialNumber);
            
            modelBuilder.Entity<Item>()
                .HasOne(c => c.ItemTemplate)
                .WithMany( c => c.Items)
                .HasForeignKey(c => c.ItemTemplateId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Item>()
                .HasMany(c => c.Documents)
                .WithMany(c=> c.Items);
        }
    }
    
    public static class ListConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<List>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<List>()
                .HasMany(c => c.Items)
                .WithMany();
            
            modelBuilder.Entity<List>()
                .HasOne(c => c.CreatedBy)
                .WithMany()
                .HasForeignKey(c => c.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<List>()
                .HasIndex(c => c.Title);
            
            modelBuilder.Entity<Item>()
                .HasIndex(c => c.CreatedById);
        }
    }
    
    public static class DocumentTypeConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DocumentType>()
                .HasKey(d => d.Id);
        }
    }
    
    public static class DocumentConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Document>()
                .HasKey(d => d.Id);

            modelBuilder.Entity<Document>()
                .HasOne(d => d.DocumentType)
                .WithMany()
                .HasForeignKey(d => d.DocumentTypeId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
    
    public static class PreCheckConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PreCheck>()
                .HasKey(d => d.Id);

            modelBuilder.Entity<PreCheck>()
                .HasMany<Item>()
                .WithOne(d => d.PreCheck)
                .HasForeignKey(d => d.PreCheckId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
    
    public static class ItemTemplateConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemTemplate>()
                .HasKey(d => d.Id);

            modelBuilder.Entity<ItemTemplate>()
                .HasMany(d => d.Sizes)
                .WithOne()
                .HasForeignKey(d => d.ItemTemplateId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ItemTemplate>()
                .HasMany(d => d.Documents)
                .WithMany(d => d.ItemTemplates);
        }
    }
    
    public static class SizeConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Size>()
                .HasKey(d => d.Id);
        }
    }
}
