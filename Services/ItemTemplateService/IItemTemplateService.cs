using Inventory.Models;

namespace Inventory.Services
{
    public interface IItemTemplateService
    {
        Task<IEnumerable<ItemTemplate>> GetAllItemTemplatesAsync();
        Task<ItemTemplate> GetItemTemplateByIdAsync(string id);
        Task<string?> CreateItemTemplateAsync(ItemTemplate itemTemplate);
        Task UpdateItemTemplateAsync(ItemTemplate itemTemplate);
        Task DeleteItemTemplateAsync(string id);
    }
}