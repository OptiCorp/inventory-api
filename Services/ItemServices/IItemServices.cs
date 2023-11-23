using Inventory.Models.DTO;

namespace Inventory.Services
{
    public interface IItemService
    {
        Task<IEnumerable<ItemResponseDto>> GetAllItemsAsync();
        Task<IEnumerable<ItemResponseDto>> GetAllItemsBySearchStringAsync(string searchString);
        Task<IEnumerable<ItemResponseDto>> GetAllItemsByUserIdAsync(string id);
        Task<ItemResponseDto> GetItemByIdAsync(string id);
        Task<IEnumerable<ItemResponseDto>> GetChildrenAsync(string id);
        // Task UpdateItemAsync(ItemUpdateDto Part);
        Task<string> CreateItemAsync(ItemCreateDto item);
        Task DeleteItemAsync(string id);
    }
}
