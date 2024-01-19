using Inventory.Models;
using Inventory.Models.DTO;

namespace Inventory.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<IEnumerable<Category>> GetAllCategoriesBySearchStringAsync(string searchString);
        Task<Category> GetCategoryByIdAsync(string id);
        Task<string?> CreateCategoryAsync(CategoryCreateDto category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(string id);
    }
}