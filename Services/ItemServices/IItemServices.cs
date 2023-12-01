using Inventory.Models.DTOs.ItemDtos;


namespace Inventory.Services
{
    public interface IItemService
    {
        Task<IEnumerable<ItemResponseDto>> GetAllItemsAsync();
        Task<IEnumerable<ItemResponseDto>> GetAllItemsBySearchStringAsync(string searchString, int page);
        Task<IEnumerable<ItemResponseDto>> GetAllItemsByUserIdAsync(string id, int page);
        Task<ItemResponseDto> GetItemByIdAsync(string id);
        Task<IEnumerable<ItemResponseDto>> GetChildrenAsync(string id);
        Task<List<string?>> CreateItemAsync(List<ItemCreateDto> item);
        Task UpdateItemAsync(ItemUpdateDto item);
        Task DeleteItemAsync(string id);
    }
}
