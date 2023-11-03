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
}
