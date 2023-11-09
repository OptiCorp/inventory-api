using Inventory.Models;
using Inventory.Models.DTO;


namespace Inventory.Utilities
{
    public interface IAssemblyUtilities
    {
        public AssemblyResponseDto AssemblyToResponseDto(Assembly? punch);
    }
}
