using Inventory.Models;

namespace Inventory.Services
{
    public interface IListService
    {
        Task<IEnumerable<List>> GetAllListsAsync();
        Task<IEnumerable<List>> GetAllListsBySearchStringAsync(string searchString, int page, string userId);
        Task<IEnumerable<List>> GetAllListsByUserIdAsync(string id, int page);
        Task<List> GetListByIdAsync(string id);
        Task<string?> CreateListAsync(List list);
        Task AddItemsToListAsync(IEnumerable<string> itemIds, string listId);
        Task RemoveItemsFromListAsync(IEnumerable<string> itemIds, string listId);
        Task UpdateListAsync(List list);
        Task DeleteListAsync(string id);
    }
}