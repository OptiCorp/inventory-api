using Inventory.Models;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                return await _context.Categories.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<IEnumerable<Category>> GetAllCategoriesBySearchStringAsync(string searchString)
        {
            try
            {
                return await _context.Categories.Where(category => category.Name.Contains(searchString))
                                            .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<Category> GetCategoryByIdAsync(string id)
        {
            try
            {
                return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<string?> CreateCategoryAsync(Category categoryCreate)
        {
            try
            {
                categoryCreate.CreatedDate = DateTime.Now;
                await _context.Categories.AddAsync(categoryCreate);
                await _context.SaveChangesAsync();
                return categoryCreate.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task UpdateCategoryAsync(Category categoryUpdate)
        {
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryUpdate.Id);
        
                if (category != null)
                {
                    category.Name = categoryUpdate.Name;
                    category.UpdatedDate = DateTime.Now;
        
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task DeleteCategoryAsync(string id)
        {
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
                if (category != null)
                {
                    _context.Categories.Remove(category);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}