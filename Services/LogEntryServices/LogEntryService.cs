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
            if (item == null)
            {
                throw new InvalidOperationException("Item not found");
            }
            var itemLogEntries = await context.LogEntries.Where(c => c.ItemId == id).ToListAsync();
            if (includeTemplateEntries == true)
            {
                var itemTemplateLogEntries = await context.LogEntries.Where(c => c.ItemTemplateId == item.ItemTemplateId).ToListAsync();
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

    public async Task<IEnumerable<LogEntry>> GetLogEntriesByItemTemplateIdAsync(string id, int page)
    {
        try
        {
            return await context.LogEntries.Where(c => c.ItemTemplateId == id).OrderByDescending(c => c.CreatedDate).Skip(page == 0 ? 0 : (page - 1) * 10)
                .Take(10).ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
