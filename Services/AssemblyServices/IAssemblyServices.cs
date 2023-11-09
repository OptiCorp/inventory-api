using Inventory.Models.DTO;

namespace Inventory.Services
{
    public interface IAssemblyServices
    {
        Task<IEnumerable<AssemblyResponseDto>> GetAllAssemblysAsync();
        Task<AssemblyResponseDto> GetAssemblyByIdAsync(string id);
        Task UpdateAssemblyAsync(AssemblyUpdateDto Assembly);
        Task<string> CreateChecklistAsync(AssemblyCreateDto Assembly);
        Task DeleteAssemblyAsync(string id);
    }
}
