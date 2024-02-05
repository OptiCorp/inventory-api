using Inventory.Models;
using Inventory.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Services;

public class SizeService(InventoryDbContext context) : ISizeService
{
    public async Task<IEnumerable<Size>> GetAllSizesAsync()
    {
        try
        {
            return await context.Sizes.ToListAsync();
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
            return await context.Sizes.FirstOrDefaultAsync(c => c.Id == id);
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

            await context.Sizes.AddAsync(size);
            await context.SaveChangesAsync();
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
            var size = await context.Sizes.FirstOrDefaultAsync(c => c.Id == sizeUpdate.Id);

            if (size != null)
            {
                size.ItemTemplateId = sizeUpdate.ItemTemplateId;
                size.Property = sizeUpdate.Property;
                size.Amount = sizeUpdate.Amount;
                size.Unit = sizeUpdate.Unit;

                await context.SaveChangesAsync();
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
            var size = await context.Sizes.FirstOrDefaultAsync(c => c.Id == id);
            if (size != null)
            {
                context.Sizes.Remove(size);
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