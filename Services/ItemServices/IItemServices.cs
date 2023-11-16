using Inventory.Models.DTO;

namespace Inventory.Services
{
    public interface IItemService
    {
        Task<IEnumerable<ItemResponseDto>> GetAllItemsAsync();
        Task<IEnumerable<ItemResponseDto>> GetAllItemsBySubassemblyIdAsync(string subassemblyId);
        Task<IEnumerable<ItemResponseDto>> GetAllItemsBySearchStringAsync(string searchString);
        Task<ItemResponseDto> GetItemByIdAsync(string id);
        Task UpdateItemAsync(ItemUpdateDto item);
        Task<string> CreateItemAsync(ItemCreateDto item);
        Task DeleteItemAsync(string id);
    }
}
