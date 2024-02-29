using Inventory.Models;
using Inventory.Models.DTO;


namespace Inventory.Services;

public interface IItemService
{
    Task<IEnumerable<Item>> GetAllItemsAsync();
    Task<IEnumerable<Item>> GetAllItemsBySearchStringAsync(string searchString, int page);
    Task<IEnumerable<Item>> GetAllItemsByUserIdAsync(string id, int page);
    Task<Item?> GetItemByIdAsync(string id);
    Task<List<Item>?> GetItemsByIdChecklistAsync(List<string> ids);
    Task<IEnumerable<Item>> GetChildrenAsync(string id);
    Task<List<string>?> CreateItemAsync(IEnumerable<ItemCreateDto> item);
    Task AddChildItemToParentAsync(string parentItemId, string childItemId);
    Task RemoveParentIdAsync(string itemId);
    Task UpdateItemAsync(string updatedById, Item item);
    Task<List<string>> DeleteItemAsync(string id, bool? deleteSubItems);
    Task<bool> IsWpIdUnique(string id);
    Task<bool> IsSerialNumberUnique(string serialNumber);
    Task ItemsCreated(List<string> ids);
    Task ItemsDeleted(List<string> ids);
}