using Inventory.Models.DTO;

namespace Inventory.Services
{
    public interface IPartService
    {
        Task<IEnumerable<PartResponseDto>> GetAllPartsAsync();
        Task<IEnumerable<PartResponseDto>> GetAllPartsBySubassemblyIdAsync(string subassemblyId);
        Task<IEnumerable<PartResponseDto>> GetAllPartsBySearchStringAsync(string searchString);
        Task<PartResponseDto> GetPartByIdAsync(string id);
        Task UpdatePartAsync(PartUpdateDto Part);
        Task<string> CreatePartAsync(PartCreateDto Part);
        Task DeletePartAsync(string id);
    }
}
