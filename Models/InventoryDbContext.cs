using Microsoft.EntityFrameworkCore;

namespace Inventory.Models;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
        : base(options)
    {
    }

    public DbSet<Material> Materials { get; set; } = null!;
    public DbSet<Assigned> Assigned { get; set; } = null!;
    public DbSet<Equipment> Equipment { get; set; } = null!;
    public DbSet<User> User { get; set; } = null!;

    public DbSet<Unit> Units { get; set; } = null!;
    public DbSet<Assembly> Assemblies { get; set; } = null!;
    public DbSet<Subassembly> Subassemblies { get; set; } = null!;
    public DbSet<Item> Items { get; set; } = null!;

}
