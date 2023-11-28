using Inventory.Models;
using Inventory.Models.DTOs.ItemDtos;


namespace Inventory.Utilities
{
    public interface IItemUtilities
    {
        public ItemResponseDto ItemToResponseDto(Item item);
    }
}
