using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Inventory.Models.DTOs.VendorDtos;
using Inventory.Utilities;

namespace Inventory.Services
{
    public class VendorService : IVendorService
    {
        private readonly InventoryDbContext _context;
        private readonly IVendorUtilities _vendorUtilities;

        public VendorService(InventoryDbContext context, IVendorUtilities vendorUtilities)
        {
            _context = context;
            _vendorUtilities = vendorUtilities;
        }

        public async Task<IEnumerable<VendorResponseDto>> GetAllVendorsAsync()
        {
            return await _context.Vendors.Include(c => c.User)
                                            .Select(c => _vendorUtilities.VendorToResponseDto(c))
                                            .ToListAsync();
        }
        
        public async Task<IEnumerable<VendorResponseDto>> GetAllVendorsBySearchStringAsync(string searchString)
        {   
            return await _context.Vendors
                .Where(vendor =>
                    vendor.Name.Contains(searchString)
                    )
                .Include(c => c.User)
                .Select(vendor => _vendorUtilities.VendorToResponseDto(vendor))
                .ToListAsync();
        }
        
        public async Task<VendorResponseDto> GetVendorByIdAsync(string id)
        {
            var vendor = await _context.Vendors
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);
        
            if (vendor == null) return null;
        
            return _vendorUtilities.VendorToResponseDto(vendor);
        }
        
        public async Task<string?> CreateVendorAsync(VendorCreateDto vendorCreateDto)
        {
            try
            {
                var vendor = new Vendor
                {
                    Name = vendorCreateDto.Name,
                    Address = vendorCreateDto.Address,
                    Email = vendorCreateDto.Email,
                    PhoneNumber = vendorCreateDto.PhoneNumber,
                    UserId = vendorCreateDto.AddedById,
                    CreatedDate = DateTime.Now
                };
                
                await _context.Vendors.AddAsync(vendor);
                await _context.SaveChangesAsync();
                return vendor.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine("Creating vendor failed.");
                return null;
            }
        }

        public async Task UpdateVendorAsync(VendorUpdateDto updatedVendor)
        {
            var vendor = await _context.Vendors.FirstOrDefaultAsync(c => c.Id == updatedVendor.Id);
        
            if (vendor != null)
            {
                vendor.Name = updatedVendor.Name;
                vendor.Address = updatedVendor.Address;
                vendor.Email = updatedVendor.Email;
                vendor.PhoneNumber = updatedVendor.PhoneNumber;
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
