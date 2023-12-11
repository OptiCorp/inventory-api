using Microsoft.EntityFrameworkCore;
using Inventory.Configuration;

namespace Inventory.Models;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
        : base(options)
    {
    }
    public DbSet<User> User { get; set; } = null!;
    public DbSet<Item> Items { get; set; } = null!;
    public DbSet<List> Lists { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Vendor> Vendors { get; set; } = null!;
    public DbSet<Location> Locations { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        UserConfigurations.Configure(modelBuilder);
        ItemConfigurations.Configure(modelBuilder);
        ListConfigurations.Configure(modelBuilder);
        
    }

}
