using Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Services
{
    public class SizeServices : ISizeService
    {
        private readonly InventoryDbContext _context;

        public SizeServices(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Size>> GetAllSizesAsync()
        {
            return await _context.Sizes.ToListAsync();
        }
        
        public async Task<Size> GetSizeByIdAsync(string id)
        {
            return await _context.Sizes.FirstOrDefaultAsync(c => c.Id == id);
        }
        
        public async Task<string?> CreateSizeAsync(Size sizeCreate)
        {
            try
            {
                await _context.Sizes.AddAsync(sizeCreate);
                await _context.SaveChangesAsync();
                return sizeCreate.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task UpdateSizeAsync(Size sizeUpdate)
        {
            var size = await _context.Sizes.FirstOrDefaultAsync(c => c.Id == sizeUpdate.Id);
        
            if (size != null)
            {
                size.ItemTemplateId = sizeUpdate.ItemTemplateId;
                size.Property = sizeUpdate.Property;
                size.Amount = sizeUpdate.Amount;
                size.Unit = sizeUpdate.Unit;
        
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteSizeAsync(string id)
        {
            var size = await _context.Sizes.FirstOrDefaultAsync(c => c.Id == id);
            if (size != null)
            {
                _context.Sizes.Remove(size);
                await _context.SaveChangesAsync();
            }
        }
    }
}
