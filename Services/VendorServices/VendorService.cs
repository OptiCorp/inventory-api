using Inventory.Models;
using Inventory.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Services;

public class VendorService(InventoryDbContext context) : IVendorService
{
    public async Task<IEnumerable<Vendor>> GetAllVendorsAsync()
    {
        try
        {
            return await context.Vendors.ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<Vendor>> GetAllVendorsBySearchStringAsync(string searchString)
    {
        try
        {
            return await context.Vendors.Where(vendor => vendor.Name != null && vendor.Name.Contains(searchString))
                .ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Vendor?> GetVendorByIdAsync(string id)
    {
        try
        {
            return await context.Vendors.FirstOrDefaultAsync(c => c.Id == id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<string?> CreateVendorAsync(VendorCreateDto vendorCreate)
    {
        try
        {
            var vendor = new Vendor
            {
                Name = vendorCreate.Name,
                CreatedById = vendorCreate.CreatedById,
                CreatedDate = DateTime.Now
            };

            await context.Vendors.AddAsync(vendor);
            await context.SaveChangesAsync();
            return vendor.Id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task UpdateVendorAsync(Vendor vendorUpdate)
    {
        try
        {
            var vendor = await context.Vendors.FirstOrDefaultAsync(c => c.Id == vendorUpdate.Id);

            if (vendor != null)
            {
                vendor.Name = vendorUpdate.Name;
                vendor.UpdatedDate = DateTime.Now;

                await context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task DeleteVendorAsync(string id)
    {
        try
        {
            var vendor = await context.Vendors.FirstOrDefaultAsync(c => c.Id == id);
            if (vendor != null)
            {
                context.Vendors.Remove(vendor);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}