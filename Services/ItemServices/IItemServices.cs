using Inventory.Models.DTO;

namespace Inventory.Services
{
    public interface IItemServices
    {
        Task<IEnumerable<ItemResponseDto>> GetAllItemsAsync();
        Task<ItemResponseDto> GetItemByIdAsync(string id);
        Task UpdateItemAsync(ItemUpdateDto item);
        Task<string> CreateItemAsync(ItemCreateDto item);
        Task DeleteItemAsync(string id);
    }
}
