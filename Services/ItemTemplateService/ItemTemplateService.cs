using Inventory.Models;
using Inventory.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Services
{
    public class ItemTemplateService : IItemTemplateService
    {
        private readonly InventoryDbContext _context;

        public ItemTemplateService(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ItemTemplate>> GetAllItemTemplatesAsync()
        {
            try
            {
                return await _context.ItemTemplates
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
                return await _context.ItemTemplates
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
                var itemTemplate = new ItemTemplate()
                {
                    ProductNumber = itemTemplateCreate.ProductNumber,
                    Type = itemTemplateCreate.Type,
                    CategoryId = itemTemplateCreate.CategoryId,
                    Revision = itemTemplateCreate.Revision,
                    CreatedById = itemTemplateCreate.CreatedById,
                    Description = itemTemplateCreate.Description,
                    CreatedDate = DateTime.Now
                };

                await _context.ItemTemplates.AddAsync(itemTemplate);
                await _context.SaveChangesAsync();
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
                var itemTemplate = await _context.ItemTemplates.Include(itemTemplate => itemTemplate.Category).FirstOrDefaultAsync(c => c.Id == itemTemplateUpdate.Id);

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
                        await _context.LogEntries.AddAsync(logEntry);
                    }

                    if (itemTemplateUpdate.CategoryId != itemTemplate.CategoryId)
                    {
                        logEntry = new LogEntry
                        {
                            ItemTemplateId = itemTemplate.Id,
                            CreatedById = updatedById,
                            Message = $"Category changed from {itemTemplate.Category?.Name} to {itemTemplateUpdate.Category?.Name}",
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
                            Message = $"Category changed from {itemTemplate.ProductNumber} to {itemTemplateUpdate.ProductNumber}",
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
                        await _context.LogEntries.AddAsync(logEntry);
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
                        await _context.LogEntries.AddAsync(logEntry);
                    }

                    itemTemplate.CreatedById = itemTemplateUpdate.CreatedById;
                    itemTemplate.UpdatedDate = DateTime.Now;

                    await _context.SaveChangesAsync();
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
                var itemTemplate = await _context.ItemTemplates.FirstOrDefaultAsync(c => c.Id == id);
                if (itemTemplate != null)
                {
                    _context.ItemTemplates.Remove(itemTemplate);
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
