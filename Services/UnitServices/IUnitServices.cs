using Inventory.Models.DTO;

namespace Inventory.Services
{
    public interface IUnitServices
    {
        Task<IEnumerable<UnitResponseDto>> GetAllUnitsAsync();
        Task<UnitResponseDto> GetUnitByIdAsync(string id);
        Task UpdateUnitAsync(UnitUpdateDto Unit);
        Task<string> CreateUnitAsync(UnitCreateDto Unit);
        Task DeleteUnitAsync(string id);
    }
}
