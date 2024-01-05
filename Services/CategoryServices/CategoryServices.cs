using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Inventory.Utilities;

namespace Inventory.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly InventoryDbContext _context;

        public CategoryService(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }
        
        public async Task<IEnumerable<Category>> GetAllCategoriesBySearchStringAsync(string searchString)
        {   
            return await _context.Categories.Where(category => category.Name.Contains(searchString))
                                            .ToListAsync();
        }
        
        public async Task<Category> GetCategoryByIdAsync(string id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }
        
        public async Task<string?> CreateCategoryAsync(Category categoryCreate)
        {
            try
            {
                await _context.Categories.AddAsync(categoryCreate);
                await _context.SaveChangesAsync();
                return categoryCreate.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task UpdateCategoryAsync(Category categoryUpdate)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryUpdate.Id);
        
            if (category != null)
            {
                category.Name = categoryUpdate.Name;
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
