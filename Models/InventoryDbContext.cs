using Microsoft.EntityFrameworkCore;
using Inventory.Configuration;

namespace Inventory.Models;

public class InventoryDbContext(DbContextOptions<InventoryDbContext> options) : DbContext(options)
{
    public DbSet<User> User { get; set; } = null!;
    public DbSet<Item> Items { get; set; } = null!;
    public DbSet<List> Lists { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Vendor> Vendors { get; set; } = null!;
    public DbSet<Location> Locations { get; set; } = null!;
    public DbSet<LogEntry> LogEntries { get; set; } = null!;
    public DbSet<Document> Documents { get; set; } = null!;
    public DbSet<DocumentType> DocumentTypes { get; set; } = null!;
    public DbSet<ItemTemplate> ItemTemplates { get; set; } = null!;
    public DbSet<PreCheck> PreChecks { get; set; } = null!;
    public DbSet<Size> Sizes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        UserConfigurations.Configure(modelBuilder);
        ItemConfigurations.Configure(modelBuilder);
        ListConfigurations.Configure(modelBuilder);
        DocumentConfigurations.Configure(modelBuilder);
        DocumentTypeConfigurations.Configure(modelBuilder);
        ItemTemplateConfigurations.Configure(modelBuilder);
        PreCheckConfigurations.Configure(modelBuilder);
        SizeConfigurations.Configure(modelBuilder);
        LocationConfigurations.Configure(modelBuilder);
        CategoryConfigurations.Configure(modelBuilder);
        VendorConfigurations.Configure(modelBuilder);
    }

}
