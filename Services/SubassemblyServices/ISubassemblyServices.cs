using Inventory.Models.DTO;

namespace Inventory.Services
{
    public interface ISubassemblyServices
    {
        Task<IEnumerable<SubassemblyResponseDto>> GetAllSubassemblysAsync();
        Task<SubassemblyResponseDto> GetSubassemblyByIdAsync(string id);
        Task UpdateSubassemblyAsync(SubassemblyUpdateDto Subassembly);
        Task<string> CreateChecklistAsync(SubassemblyCreateDto Subassembly);
        Task DeleteSubassemblyAsync(string id);
    }
}
