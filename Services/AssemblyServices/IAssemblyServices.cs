using Inventory.Models.DTO;

namespace Inventory.Services
{
    public interface IAssemblyService
    {
        Task<IEnumerable<AssemblyResponseDto>> GetAllAssembliesAsync();
        Task<IEnumerable<AssemblyResponseDto>> GetAllAssembliesByUnitIdAsync(string unitId);
        Task<IEnumerable<AssemblyResponseDto>> GetAllAssembliesBySearchStringAsync(string searchString);
        Task<AssemblyResponseDto> GetAssemblyByIdAsync(string id);
        Task UpdateAssemblyAsync(AssemblyUpdateDto Assembly);
        Task<string> CreateAssemblyAsync(AssemblyCreateDto Assembly);
        Task DeleteAssemblyAsync(string id);
    }
}
