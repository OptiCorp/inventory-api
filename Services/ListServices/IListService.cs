using Inventory.Models;
using Inventory.Models.DTO;

namespace Inventory.Services;

public interface IListService
{
    Task<IEnumerable<List>> GetAllListsAsync();
    Task<IEnumerable<List>> GetAllListsBySearchStringAsync(string searchString, int page, string userId);
    Task<IEnumerable<List>> GetAllListsByUserIdAsync(string id, int page);
    Task<List?> GetListByIdAsync(string id);
    Task<string?> CreateListAsync(ListCreateDto list);
    Task AddItemsToListAsync(IEnumerable<string> itemIds, string listId, bool? addSubItems);
    Task RemoveItemsFromListAsync(IEnumerable<string> itemIds, string listId, bool? removeSubItems);
    Task UpdateListAsync(List list);
    Task DeleteListAsync(string id);
}