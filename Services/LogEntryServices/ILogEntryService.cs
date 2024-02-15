using Inventory.Models;

namespace Inventory.Services;

public interface ILogEntryService
{
    Task<IEnumerable<LogEntry?>> GetLogEntriesByItemIdAsync(string id, int page, bool? includeTemplateEntries);
    Task<IEnumerable<LogEntry?>> GetLogEntriesByItemTemplateIdAsync(string id, int page);
}