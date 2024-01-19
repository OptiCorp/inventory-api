using Inventory.Models;

namespace Inventory.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<IEnumerable<Category>> GetAllCategoriesBySearchStringAsync(string searchString);
        Task<Category> GetCategoryByIdAsync(string id);
        Task<string?> CreateCategoryAsync(Category item);
        Task UpdateCategoryAsync(Category item);
        Task DeleteCategoryAsync(string id);
    }
}