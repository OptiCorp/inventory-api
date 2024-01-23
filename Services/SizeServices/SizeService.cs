using Inventory.Models;
using Inventory.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Services
{
    public class SizeService : ISizeService
    {
        private readonly InventoryDbContext _context;

        public SizeService(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Size>> GetAllSizesAsync()
        {
            try
            {
                return await _context.Sizes.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<Size?> GetSizeByIdAsync(string id)
        {
            try
            {
                return await _context.Sizes.FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<string?> CreateSizeAsync(SizeCreateDto sizeCreate)
        {
            try
            {
                var size = new Size
                {
                    ItemTemplateId = sizeCreate.ItemTemplateId,
                    Property = sizeCreate.Property,
                    Amount = sizeCreate.Amount,
                    Unit = sizeCreate.Unit
                };
                
                await _context.Sizes.AddAsync(size);
                await _context.SaveChangesAsync();
                return size.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task UpdateSizeAsync(Size sizeUpdate)
        {
            try
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
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task DeleteSizeAsync(string id)
        {
            try
            {
                var size = await _context.Sizes.FirstOrDefaultAsync(c => c.Id == id);
                if (size != null)
                {
                    _context.Sizes.Remove(size);
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
