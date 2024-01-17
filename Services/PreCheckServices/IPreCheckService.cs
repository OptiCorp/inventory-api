using Inventory.Models;

namespace Inventory.Services
{
    public interface IPreCheckService
    {
        Task<IEnumerable<PreCheck>> GetAllPreChecksAsync();
        Task<PreCheck> GetPreCheckByIdAsync(string id);
        Task<string?> CreatePreCheckAsync(PreCheck preCheck);
        Task UpdatePreCheckAsync(PreCheck preCheck);
        Task DeletePreCheckAsync(string id);
    }
}