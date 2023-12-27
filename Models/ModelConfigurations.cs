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

            modelBuilder.Entity<Item>()
                .HasIndex(c => c.Description);
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
}
