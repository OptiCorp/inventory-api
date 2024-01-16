using Inventory.Models;
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
            return await _context.ItemTemplates
                .Include(c => c.Documents)
                .Include(c => c.Sizes)
                .Include(c => c.CreatedBy)
                .ToListAsync();
        }
        

        public async Task<ItemTemplate> GetItemTemplateByIdAsync(string id)
        {
            return await _context.ItemTemplates
                .Include(c => c.Documents)
                .Include(c => c.Sizes)
                .Include(c => c.CreatedBy)
                .FirstOrDefaultAsync(c => c.Id == id);
            
        }
        
        public async Task<string?> CreateItemTemplateAsync(ItemTemplate itemTemplate)
        {
            try
            {
                itemTemplate.CreatedDate = DateTime.Now;
                await _context.ItemTemplates.AddAsync(itemTemplate);
                await _context.SaveChangesAsync();
                return itemTemplate.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        
        public async Task UpdateItemTemplateAsync(ItemTemplate itemTemplateUpdate)
        {
            var itemTemplate = await _context.ItemTemplates.FirstOrDefaultAsync(c => c.Id == itemTemplateUpdate.Id);
        
            if (itemTemplate != null)
            {
                itemTemplate.Name = itemTemplateUpdate.Name;
                itemTemplate.Type = itemTemplateUpdate.Type;
                itemTemplate.ProductNumber = itemTemplateUpdate.ProductNumber;
                itemTemplate.Revision = itemTemplateUpdate.Revision;
                itemTemplate.Description = itemTemplateUpdate.Description;
                itemTemplate.CreatedById = itemTemplateUpdate.CreatedById;
                itemTemplate.UpdatedDate = DateTime.Now;
        
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteItemTemplateAsync(string id)
        {
            var itemTemplate = await _context.ItemTemplates.FirstOrDefaultAsync(c => c.Id == id);
            if (itemTemplate != null)
            {
                _context.ItemTemplates.Remove(itemTemplate);
                await _context.SaveChangesAsync();
            }
        }
    }
}
