using Inventory.Models;
using Inventory.Models.DTO;


namespace Inventory.Utilities
{
    public interface IItemUtilities
    {
        public ItemResponseDto ItemToResponseDto(Item? punch);
    }
}
