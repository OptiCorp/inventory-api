using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Inventory.Utilities;

namespace Inventory.Services
{
    public class VendorService : IVendorService
    {
        private readonly InventoryDbContext _context;

        public VendorService(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Vendor>> GetAllVendorsAsync()
        {
            return await _context.Vendors.ToListAsync();
        }
        
        public async Task<IEnumerable<Vendor>> GetAllVendorsBySearchStringAsync(string searchString)
        {   
            return await _context.Vendors.Where(vendor => vendor.Name.Contains(searchString))
                                        .ToListAsync();
        }
        
        public async Task<Vendor> GetVendorByIdAsync(string id)
        {
            return await _context.Vendors.FirstOrDefaultAsync(c => c.Id == id);
        }
        
        public async Task<string?> CreateVendorAsync(Vendor vendorCreate)
        {
            try
            {
                vendorCreate.CreatedDate = DateTime.Now;
                await _context.Vendors.AddAsync(vendorCreate);
                await _context.SaveChangesAsync();
                return vendorCreate.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task UpdateVendorAsync(Vendor vendorUpdate)
        {
            var vendor = await _context.Vendors.FirstOrDefaultAsync(c => c.Id == vendorUpdate.Id);
        
            if (vendor != null)
            {
                vendor.Name = vendorUpdate.Name;
                vendor.UpdatedDate = DateTime.Now;
        
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteVendorAsync(string id)
        {
            var vendor = await _context.Vendors.FirstOrDefaultAsync(c => c.Id == id);
            if (vendor != null)
            {
                _context.Vendors.Remove(vendor);
                await _context.SaveChangesAsync();
            }
        }
    }
}
