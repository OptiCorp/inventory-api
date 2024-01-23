using Inventory.Models;
using Inventory.Models.DTO;

namespace Inventory.Services
{
    public interface IVendorService
    {
        Task<IEnumerable<Vendor>> GetAllVendorsAsync();
        Task<IEnumerable<Vendor>> GetAllVendorsBySearchStringAsync(string searchString);
        Task<Vendor?> GetVendorByIdAsync(string id);
        Task<string?> CreateVendorAsync(VendorCreateDto vendor);
        Task UpdateVendorAsync(Vendor vendor);
        Task DeleteVendorAsync(string id);
    }
}