using Inventory.Models;
using Inventory.Models.DTO;

namespace Inventory.Services
{
    public interface IItemTemplateService
    {
        Task<IEnumerable<ItemTemplate>> GetAllItemTemplatesAsync();
        Task<ItemTemplate?> GetItemTemplateByIdAsync(string id);
        Task<string?> CreateItemTemplateAsync(ItemTemplateCreateDto itemTemplate);
        Task UpdateItemTemplateAsync(ItemTemplate itemTemplate, string updatedById);
        Task DeleteItemTemplateAsync(string id);
    }
}