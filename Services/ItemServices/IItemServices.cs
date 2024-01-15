using Inventory.Models;


namespace Inventory.Services
{
    public interface IItemService
    {
        Task<IEnumerable<Item>> GetAllItemsAsync();
        Task<IEnumerable<Item>> GetAllItemsBySearchStringAsync(string searchString, int page, string? type);
        Task<IEnumerable<Item>> GetAllItemsNotInListBySearchStringAsync(string searchString,string listId, int page);
        Task<IEnumerable<Item>> GetAllItemsByUserIdAsync(string id, int page);
        Task<Item> GetItemByIdAsync(string id);
        Task<IEnumerable<Item>> GetChildrenAsync(string id);
        Task<List<string?>> CreateItemAsync(List<Item> item);
        Task AddChildItemToParentAsync(string parentItemId, string childItemId);
        Task RemoveParentIdAsync(string itemId);
        Task UpdateItemAsync(string updatedById, Item item);
        Task DeleteItemAsync(string id);
        Task<bool> IsWpIdUnique(string id);
    }
}
