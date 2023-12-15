using Inventory.Models.DTOs.CategoryDTOs;

namespace Inventory.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync();
        Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesBySearchStringAsync(string searchString);
        Task<CategoryResponseDto> GetCategoryByIdAsync(string id);
        Task<string?> CreateCategoryAsync(CategoryCreateDto item);
        Task UpdateCategoryAsync(CategoryUpdateDto item);
        Task DeleteCategoryAsync(string id);
    }
}