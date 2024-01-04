using Microsoft.EntityFrameworkCore;
using Inventory.Models;

namespace Inventory.Configuration
{

    public static class DocumentationConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Documentation>()
                .HasKey(d => d.Id);

            modelBuilder.Entity<Documentation>()
                .HasOne(d => d.Item)
                .WithMany()
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
    
    public static class UserConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
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
                .WithMany()
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Item>()
                .HasMany(c => c.Children)
                .WithOne(c => c.Parent);
            
            modelBuilder.Entity<Item>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Item>()
                .HasIndex(c => c.WpId);

            modelBuilder.Entity<Item>()
                .HasIndex(c => c.SerialNumber);

            // modelBuilder.Entity<Item>()
            //     .HasIndex(c => c.Description);
            
            modelBuilder.Entity<Item>()
                .HasOne(c => c.ItemTemplate)
                .WithMany()
                .HasForeignKey(c => c.ItemTemplateId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Item>()
                .HasMany(c => c.Documents)
                .WithMany();
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
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<List>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<List>()
                .HasIndex(c => c.Title);
            
            modelBuilder.Entity<Item>()
                .HasIndex(c => c.UserId);
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
                .HasMany(d => d.Items)
                .WithOne();
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
                .WithMany();
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
