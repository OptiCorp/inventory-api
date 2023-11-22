using Inventory.Models;
using Inventory.Models.DTO;


namespace Inventory.Utilities
{
    public interface IPartUtilities
    {
        public PartResponseDto PartToResponseDto(Part part);
    }
}
