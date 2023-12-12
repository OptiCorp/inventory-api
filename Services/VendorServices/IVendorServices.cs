using Inventory.Models.DTOs.VendorDtos;

namespace Inventory.Services
{
    public interface IVendorService
    {
        Task<IEnumerable<VendorResponseDto>> GetAllVendorsAsync();
        Task<IEnumerable<VendorResponseDto>> GetAllVendorsBySearchStringAsync(string searchString);
        Task<VendorResponseDto> GetVendorByIdAsync(string id);
        Task<string?> CreateVendorAsync(VendorCreateDto item);
        Task UpdateVendorAsync(VendorUpdateDto item);
        Task DeleteVendorAsync(string id);
    }
}