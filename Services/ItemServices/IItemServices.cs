using Inventory.Models.DTOs.ItemDTOs;


namespace Inventory.Services
{
    public interface IItemService
    {
        Task<IEnumerable<ItemResponseDto>> GetAllItemsAsync();
        Task<IEnumerable<ItemResponseDto>> GetAllItemsBySearchStringAsync(string searchString, int page, string? type);
        Task<IEnumerable<ItemResponseDto>> GetAllItemsNotInListBySearchStringAsync(string searchString, string listId, int page);
        Task<IEnumerable<ItemResponseDto>> GetAllItemsByUserIdAsync(string id, int page);
        Task<ItemResponseDto> GetItemByIdAsync(string id);
        Task<IEnumerable<ItemResponseDto>> GetChildrenAsync(string id);
        Task<List<string?>> CreateItemAsync(List<ItemCreateDto> item);
        Task RemoveParentIdAsync(string itemId);
        Task UpdateItemAsync(string updatedById, ItemUpdateDto item);
        Task DeleteItemAsync(string id);
        Task<bool> IsWpIdUnique(string id);
    }
}
