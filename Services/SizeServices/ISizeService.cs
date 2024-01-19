using Inventory.Models;
using Inventory.Models.DTO;

namespace Inventory.Services
{
    public interface ISizeService
    {
        Task<IEnumerable<Size>> GetAllSizesAsync();
        Task<Size> GetSizeByIdAsync(string id);
        Task<string?> CreateSizeAsync(SizeCreateDto size);
        Task UpdateSizeAsync(Size size);
        Task DeleteSizeAsync(string id);
    }
}