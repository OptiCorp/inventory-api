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
    public DbSet<Unit> Units { get; set; } = null!;
    public DbSet<Assembly> Assemblies { get; set; } = null!;
    public DbSet<Subassembly> Subassemblies { get; set; } = null!;
    public DbSet<Part> Parts { get; set; } = null!;
    public DbSet<Item> Items { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        UserConfigurations.Configure(modelBuilder);
        AssemblyConfigurations.Configure(modelBuilder);
        PartConfigurations.Configure(modelBuilder);
        SubassemblyConfigurations.Configure(modelBuilder);
        UnitConfigurations.Configure(modelBuilder);
        ItemConfigurations.Configure(modelBuilder);

    }

}
