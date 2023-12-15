using Inventory.Models;
using Inventory.Models.DTOs.ItemDTOs;


namespace Inventory.Utilities
{
    public interface IItemUtilities
    {
        public ItemResponseDto ItemToResponseDto(Item item);
    }
}
