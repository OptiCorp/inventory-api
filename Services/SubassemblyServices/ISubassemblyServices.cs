using Inventory.Models.DTO;

namespace Inventory.Services
{
    public interface ISubassemblyService
    {
        Task<IEnumerable<SubassemblyResponseDto>> GetAllSubassembliesAsync();
        Task<SubassemblyResponseDto> GetSubassemblyByIdAsync(string id);
        Task UpdateSubassemblyAsync(SubassemblyUpdateDto Subassembly);
        Task<string> CreateSubassemblyAsync(SubassemblyCreateDto Subassembly);
        Task DeleteSubassemblyAsync(string id);
    }
}
