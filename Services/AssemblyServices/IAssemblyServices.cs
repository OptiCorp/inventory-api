using Inventory.Models.DTO;

namespace Inventory.Services
{
    public interface IAssemblyServices
    {
        Task<IEnumerable<AssemblyResponseDto>> GetAllAssembliesAsync();
        Task<AssemblyResponseDto> GetAssemblyByIdAsync(string id);
        Task UpdateAssemblyAsync(AssemblyUpdateDto Assembly);
        Task<string> CreateAssemblyAsync(AssemblyCreateDto Assembly);
        Task DeleteAssemblyAsync(string id);
    }
}
