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
                    Name = itemTemplateCreate.Name,
                    ProductNumber = itemTemplateCreate.ProductNumber,
                    Type = itemTemplateCreate.Type,
                    CategoryId = itemTemplateCreate.CreatedById,
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

        public async Task UpdateItemTemplateAsync(ItemTemplate itemTemplateUpdate)
        {
            try
            {
                var itemTemplate = await _context.ItemTemplates.FirstOrDefaultAsync(c => c.Id == itemTemplateUpdate.Id);

                if (itemTemplate != null)
                {
                    itemTemplate.Name = itemTemplateUpdate.Name;
                    itemTemplate.Type = itemTemplateUpdate.Type;
                    itemTemplate.CategoryId = itemTemplateUpdate.CategoryId;
                    itemTemplate.ProductNumber = itemTemplateUpdate.ProductNumber;
                    itemTemplate.Revision = itemTemplateUpdate.Revision;
                    itemTemplate.Description = itemTemplateUpdate.Description;
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
