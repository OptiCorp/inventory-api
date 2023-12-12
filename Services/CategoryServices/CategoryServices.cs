using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Inventory.Models.DTOs.CategoryDtos;
using Inventory.Utilities;

namespace Inventory.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly InventoryDbContext _context;
        private readonly ICategoryUtilities _categoryUtilities;

        public CategoryService(InventoryDbContext context, ICategoryUtilities categoryUtilities)
        {
            _context = context;
            _categoryUtilities = categoryUtilities;
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync()
        {
            return await _context.Categories.Include(c => c.User)
                                            .Select(c => _categoryUtilities.CategoryToResponseDto(c))
                                            .ToListAsync();
        }
        
        public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesBySearchStringAsync(string searchString)
        {   
            return await _context.Categories
                .Where(category =>
                    category.Name.Contains(searchString)
                    )
                .Include(c => c.User)
                .Select(category => _categoryUtilities.CategoryToResponseDto(category))
                .ToListAsync();
        }
        
        public async Task<CategoryResponseDto> GetCategoryByIdAsync(string id)
        {
            var category = await _context.Categories
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);
        
            if (category == null) return null;
        
            return _categoryUtilities.CategoryToResponseDto(category);
        }
        
        public async Task<string?> CreateCategoryAsync(CategoryCreateDto categoryCreateDto)
        {
            try
            {
                var category = new Category
                {
                    Name = categoryCreateDto.Name,
                    UserId = categoryCreateDto.AddedById,
                    CreatedDate = DateTime.Now
                };
                
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return category.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine("Creating category failed.");
                return null;
            }
        }

        public async Task UpdateCategoryAsync(CategoryUpdateDto updatedCategory)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == updatedCategory.Id);
        
            if (category != null)
            {
                category.Name = updatedCategory.Name;
                category.UpdatedDate = DateTime.Now;
        
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteCategoryAsync(string id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
