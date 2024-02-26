using Inventory.Models;
using Inventory.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Services;

public class ItemTemplateService(InventoryDbContext context) : IItemTemplateService
{
    public async Task<IEnumerable<ItemTemplate>> GetAllItemTemplatesAsync()
    {
        try
        {
            return await context.ItemTemplates
                .Include(c => c.Category)
                .Include(c => c.Documents)
                .Include(c => c.Sizes)
                .Include(c => c.CreatedBy)
                .ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    public async Task<ItemTemplate?> GetItemTemplateByIdAsync(string id)
    {
        try
        {
            return await context.ItemTemplates
                .Include(c => c.Category)
                .Include(c => c.Documents)
                .Include(c => c.Sizes)
                .Include(c => c.CreatedBy)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<ItemTemplate>> GetItemTemplateBySearchStringAsync(string searchString, int page)
    {
        try
        {
            var result = await context.ItemTemplates
                .Where(c => c.Type != null && c.Description != null &&
                            (c.Type.Contains(searchString) || c.Description.Contains(searchString))).Include(c => c.Category).OrderBy(c => c.Id)
                .Take(page * 10).ToListAsync();
            if (result.Count >= page * 10)
            {
                return result;
            }

            var remainingItemTemplateCount = page * 10 - result.Count;
            return result;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<string?> CreateItemTemplateAsync(ItemTemplateCreateDto itemTemplateCreate)
    {
        try
        {
            var itemTemplate = new ItemTemplate
            {
                ProductNumber = itemTemplateCreate.ProductNumber,
                Type = itemTemplateCreate.Type,
                CategoryId = itemTemplateCreate.CategoryId,
                Revision = itemTemplateCreate.Revision,
                CreatedById = itemTemplateCreate.CreatedById,
                Description = itemTemplateCreate.Description,
                CreatedDate = DateTime.Now
            };

            await context.ItemTemplates.AddAsync(itemTemplate);
            await context.SaveChangesAsync();
            return itemTemplate.Id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task UpdateItemTemplateAsync(ItemTemplate itemTemplateUpdate, string updatedById)
    {
        try
        {
            var itemTemplate = await context.ItemTemplates.Include(itemTemplate => itemTemplate.Category).FirstOrDefaultAsync(c => c.Id == itemTemplateUpdate.Id);

            if (itemTemplate?.Id != null)
            {
                if (itemTemplateUpdate.Type != itemTemplate.Type)
                {
                    await CreateLogEntryAsync(itemTemplate.Id, updatedById,
                        $"Type changed from {itemTemplate.Type} to {itemTemplateUpdate.Type}");
                    itemTemplate.Type = itemTemplateUpdate.Type;
                }

                if (itemTemplateUpdate.CategoryId != itemTemplate.CategoryId)
                {
                    var newCategory =
                        await context.Categories.FirstOrDefaultAsync(c => c.Id == itemTemplateUpdate.CategoryId);

                    await CreateLogEntryAsync(itemTemplate.Id, updatedById,
                        $"Category changed from {itemTemplate.Category?.Name} to {newCategory?.Name}");
                    itemTemplate.CategoryId = itemTemplateUpdate.CategoryId;
                }

                if (itemTemplateUpdate.ProductNumber != itemTemplate.ProductNumber)
                {
                    await CreateLogEntryAsync(itemTemplate.Id, updatedById,
                        $"Product number changed from {itemTemplate.ProductNumber} to {itemTemplateUpdate.ProductNumber}");
                    itemTemplate.ProductNumber = itemTemplateUpdate.ProductNumber;
                }

                if (itemTemplateUpdate.Revision != itemTemplate.Revision)
                {
                    await CreateLogEntryAsync(itemTemplate.Id, updatedById,
                        $"Revision changed from {itemTemplate.Revision} to {itemTemplateUpdate.Revision}");
                    itemTemplate.Revision = itemTemplateUpdate.Revision;
                }

                if (itemTemplateUpdate.Description != itemTemplate.Description)
                {
                    await CreateLogEntryAsync(itemTemplate.Id, updatedById,
                        "Description updated");
                    itemTemplate.Description = itemTemplateUpdate.Description;
                }

                itemTemplate.CreatedById = itemTemplateUpdate.CreatedById;
                itemTemplate.UpdatedDate = DateTime.Now;

                await context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task DeleteItemTemplateAsync(string id)
    {
        try
        {
            var itemTemplate = await context.ItemTemplates.FirstOrDefaultAsync(c => c.Id == id);
            if (itemTemplate != null)
            {
                context.ItemTemplates.Remove(itemTemplate);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task CreateLogEntryAsync(string itemTemplateId, string createdById, string message)
    {
        try
        {
            var logEntry = new LogEntry
            {
                ItemTemplateId = itemTemplateId,
                CreatedById = createdById,
                Message = message,
                CreatedDate = DateTime.Now
            };

            await context.LogEntries.AddAsync(logEntry);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}