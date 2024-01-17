using Inventory.Models;

namespace Inventory.Services
{
    public interface IVendorService
    {
        Task<IEnumerable<Vendor>> GetAllVendorsAsync();
        Task<IEnumerable<Vendor>> GetAllVendorsBySearchStringAsync(string searchString);
        Task<Vendor> GetVendorByIdAsync(string id);
        Task<string?> CreateVendorAsync(Vendor vendor);
        Task UpdateVendorAsync(Vendor vendor);
        Task DeleteVendorAsync(string id);
    }
}