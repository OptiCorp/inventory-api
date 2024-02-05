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
                .Include(c => c.LogEntries)
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
                .Include(c => c.LogEntries)
                .FirstOrDefaultAsync(c => c.Id == id);
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

            if (itemTemplate != null)
            {
                LogEntry logEntry;
                if (itemTemplateUpdate.Type != itemTemplate.Type)
                {
                    logEntry = new LogEntry
                    {
                        ItemTemplateId = itemTemplate.Id,
                        CreatedById = updatedById,
                        Message = $"Type changed from {itemTemplate.Type} to {itemTemplateUpdate.Type}",
                        CreatedDate = DateTime.Now
                    };
                    itemTemplate.Type = itemTemplateUpdate.Type;
                    await context.LogEntries.AddAsync(logEntry);
                }

                if (itemTemplateUpdate.CategoryId != itemTemplate.CategoryId)
                {
                        var newCategory =
                            await _context.Categories.FirstOrDefaultAsync(c => c.Id == itemTemplateUpdate.CategoryId);

                        logEntry = new LogEntry
                        {
                            ItemTemplateId = itemTemplate.Id,
                            CreatedById = updatedById,
                            Message = $"Category changed from {itemTemplate.Category?.Name} to {newCategory?.Name}",
                            CreatedDate = DateTime.Now
                        };
                        itemTemplate.CategoryId = itemTemplateUpdate.CategoryId;
                        await _context.LogEntries.AddAsync(logEntry);
                    }
                    
                    if (itemTemplateUpdate.ProductNumber != itemTemplate.ProductNumber)
                    {
                        logEntry = new LogEntry
                        {
                            ItemTemplateId = itemTemplate.Id,
                            CreatedById = updatedById,
                            Message = $"Product number changed from {itemTemplate.ProductNumber} to {itemTemplateUpdate.ProductNumber}",
                            CreatedDate = DateTime.Now
                        };
                        itemTemplate.ProductNumber = itemTemplateUpdate.ProductNumber;
                        await _context.LogEntries.AddAsync(logEntry);
                    }

                if (itemTemplateUpdate.Revision != itemTemplate.Revision)
                {
                    logEntry = new LogEntry
                    {
                        ItemTemplateId = itemTemplate.Id,
                        CreatedById = updatedById,
                        Message = $"Revision changed from {itemTemplate.Revision} to {itemTemplateUpdate.Revision}",
                        CreatedDate = DateTime.Now
                    };
                    itemTemplate.Revision = itemTemplateUpdate.Revision;
                    await context.LogEntries.AddAsync(logEntry);
                }

                if (itemTemplateUpdate.Description != itemTemplate.Description)
                {
                    logEntry = new LogEntry
                    {
                        ItemTemplateId = itemTemplate.Id,
                        CreatedById = updatedById,
                        Message = "Description updated",
                        CreatedDate = DateTime.Now
                    };
                    itemTemplate.Description = itemTemplateUpdate.Description;
                    await context.LogEntries.AddAsync(logEntry);
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
}