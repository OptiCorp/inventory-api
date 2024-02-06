using Inventory.Models;
using Inventory.Models.DTO;

namespace Inventory.Services;

public interface IPreCheckService
{
    Task<IEnumerable<PreCheck>> GetAllPreChecksAsync();
    Task<PreCheck?> GetPreCheckByIdAsync(string id);
    Task<string?> CreatePreCheckAsync(PreCheckCreateDto preCheck);
    Task UpdatePreCheckAsync(PreCheck preCheck);
    Task DeletePreCheckAsync(string id);
}