using Inventory.Models;
using Microsoft.EntityFrameworkCore;
namespace Inventory.Services;

public class LogEntryService(InventoryDbContext context) : ILogEntryService
{
    public async Task<IEnumerable<LogEntry?>> GetLogEntriesByItemIdAsync(string id, int page, bool? includeTemplateEntries)
    {
        try
        {
            var item = await context.Items.FirstOrDefaultAsync(c => c.Id == id);
            var itemLogEntries = await context.LogEntries.Where(c => c.ItemId == id).Include(c => c.CreatedBy).ToListAsync();

            if (includeTemplateEntries == true)
            {
                var itemTemplateLogEntries = await context.LogEntries.Where(c => item != null && c.ItemTemplateId == item.ItemTemplateId).Include(c => c.CreatedBy).ToListAsync();
                itemLogEntries.AddRange(itemTemplateLogEntries);
            }

            return itemLogEntries.OrderByDescending(c => c.CreatedDate).Skip(page == 0 ? 0 : (page - 1) * 10).Take(10);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<LogEntry?>> GetLogEntriesByItemTemplateIdAsync(string id, int page)
    {
        try
        {
            return await context.LogEntries.Where(c => c.ItemTemplateId == id).Include(c => c.CreatedBy).OrderByDescending(c => c.CreatedDate).Skip(page == 0 ? 0 : (page - 1) * 10)
                .Take(10).ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
