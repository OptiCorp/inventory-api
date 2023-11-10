using Inventory.Models;
using Inventory.Models.DTO;


namespace Inventory.Utilities
{
    public interface ISubassemblyUtilities
    {
        public SubassemblyResponseDto SubassemblyToResponseDto(Subassembly subassembly);
    }
}
