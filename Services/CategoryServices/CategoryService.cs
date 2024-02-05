using Inventory.Models;
using Inventory.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Services;

public class CategoryService(InventoryDbContext context) : ICategoryService
{
    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        try
        {
            return await context.Categories.ToListAsync();
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
            return await context.Categories.Where(category => category.Name != null && category.Name.Contains(searchString))
                .ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Category?> GetCategoryByIdAsync(string id)
    {
        try
        {
            return await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<string?> CreateCategoryAsync(CategoryCreateDto categoryCreate)
    {
        try
        {
            var category = new Category
            {
                Name = categoryCreate.Name,
                CreatedById = categoryCreate.CreatedById,
                CreatedDate = DateTime.Now
            };

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
            return category.Id;
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
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == categoryUpdate.Id);

            if (category != null)
            {
                category.Name = categoryUpdate.Name;
                category.UpdatedDate = DateTime.Now;

                await context.SaveChangesAsync();
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
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category != null)
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}