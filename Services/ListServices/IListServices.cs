using Inventory.Models.DTOs.ListDtos;

namespace Inventory.Services
{
    public interface IListService
    {
        Task<IEnumerable<ListResponseDto>> GetAllListsAsync();
        Task<IEnumerable<ListResponseDto>> GetAllListsBySearchStringAsync(string searchString, int page, string userId);
        Task<IEnumerable<ListResponseDto>> GetAllListsByUserIdAsync(string id, int page);
        Task<ListResponseDto> GetListByIdAsync(string id);
        Task<string?> CreateListAsync(ListCreateDto list);
        Task AddItemsToListAsync(IEnumerable<string> itemIds, string listId);
        Task RemoveItemsFromListAsync(IEnumerable<string> itemIds);
        Task UpdateListAsync(ListUpdateDto list);
        Task DeleteListAsync(string id);
    }
}