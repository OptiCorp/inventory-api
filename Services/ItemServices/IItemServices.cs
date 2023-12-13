using Inventory.Models.DTOs.ItemDtos;


namespace Inventory.Services
{
    public interface IItemService
    {
        Task<IEnumerable<ItemResponseDto>> GetAllItemsAsync();
        Task<IEnumerable<ItemResponseDto>> GetAllItemsBySearchStringAsync(string searchString, int page, string type);
        Task<IEnumerable<ItemResponseDto>> GetAllItemsNotInListBySearchStringAsync(string searchString,string listId, int page);
        Task<IEnumerable<ItemResponseDto>> GetAllItemsByUserIdAsync(string id, int page);
        Task<ItemResponseDto> GetItemByIdAsync(string id);
        Task<IEnumerable<ItemResponseDto>> GetChildrenAsync(string id);
        Task<List<string?>> CreateItemAsync(List<ItemCreateDto> item);
        Task UpdateItemAsync(string updatedById, ItemUpdateDto item);
        Task DeleteItemAsync(string id);
    }
}
