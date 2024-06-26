using System.Collections.ObjectModel;
using Azure.Messaging.ServiceBus;
using Inventory.Models;
using Inventory.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Inventory.Utilities;
using checklist_inventory_contracts.Items;
using System.Text.Json;
using Inventory.Configuration;

namespace Inventory.Services;

public class ItemService(InventoryDbContext context, IGeneralUtilities generalUtilities) : IItemService
{
    public async Task<IEnumerable<Item>> GetAllItemsAsync()
    {
        try
        { 
            return await context.Items
                .ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<Item>> GetAllItemsBySearchStringAsync(string searchString, int page)
    {
        try
        {
            var result = await context.Items
                .Where(c => c.SerialNumber != null && c.WpId != null && (c.WpId.Contains(searchString) || c.SerialNumber.Contains(searchString)))
                .Include(c => c.ItemTemplate)
                .ThenInclude(c => c!.Category)
                .Include(c => c.Parent)
                .Include(c => c.Children)
                .Include(c => c.CreatedBy)
                .Include(c => c.Vendor)
                .Include(c => c.Location)
                .OrderBy(c => c.Id)
                .Take(page * 10)
                .ToListAsync();

            if (result.Count >= page * 10)
            {
                return result;
            }

            var remainingItemsCount = page * 10 - result.Count;

            var templateIds = await context.ItemTemplates
                .Where(c => c.Description != null && c.Description.Contains(searchString))
                .OrderBy(c => c.Id)
                .Select(c => c.Id)
                .ToListAsync();

            var items = await context.Items
                .Where(c => templateIds.Contains(c.ItemTemplateId))
                .Include(c => c.ItemTemplate)
                .ThenInclude(c => c!.Category)
                .Include(c => c.Parent)
                .Include(c => c.Children)
                .Include(c => c.CreatedBy)
                .Include(c => c.Vendor)
                .Include(c => c.Location)
                .OrderBy(c => c.Id)
                .Take(remainingItemsCount)
                .ToListAsync();

            result.AddRange(items);

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<Item>> GetAllItemsByUserIdAsync(string id, int page)
    {
        try
        {
            return await context.Items.Where(c => c.CreatedById == id)
                .Include(c => c.ItemTemplate)
                .ThenInclude(c => c!.Category)
                .Include(c => c.Parent)
                .Include(c => c.Children)
                .Include(c => c.CreatedBy)
                .Include(c => c.Vendor)
                .Include(c => c.Location)
                .OrderByDescending(c => c.CreatedDate)
                .Skip(page == 0 ? 0 : (page - 1) * 10)
                .Take(10)
                .ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<Item>> GetChildrenAsync(string parentId)
    {
        try
        {
            return await context.Items.Where(c => c.ParentId == parentId)
                .Include(c => c.ItemTemplate)
                .ThenInclude(c => c!.Category)
                .Include(c => c.Parent)
                .Include(c => c.Children)
                .Include(c => c.CreatedBy)
                .Include(c => c.Vendor)
                .Include(c => c.Location)
                .ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Item?> GetItemByIdAsync(string id)
    {
        try
        {
            return await context.Items
                .Include(c => c.ItemTemplate)
                .ThenInclude(c => c!.Category)
                .Include(c => c.Parent)
                .Include(c => c.Children)
                .Include(c => c.CreatedBy)
                .Include(c => c.Vendor)
                .Include(c => c.Location)
                .FirstOrDefaultAsync(c => c.Id == id);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<Item>?> GetItemsByIdChecklistAsync(List<string> ids)
    {
        try
        {
            return await context.Items
                .Include(c => c.ItemTemplate)
                .ThenInclude(c => c!.Category)
                .Include(c => c.Parent)
                .Include(c => c.Children)
                .Include(c => c.CreatedBy)
                .Include(c => c.Vendor)
                .Include(c => c.Location)
                .Where(c => c.Id != null && ids.Contains(c.Id))
                .ToListAsync();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<string>?> CreateItemAsync(IEnumerable<ItemCreateDto> itemsCreate)
    {
        try
        {
            var createdItemsIds = new List<string>();
            foreach (var item in itemsCreate.Select(itemCreate => new Item
            {
                WpId = itemCreate.WpId,
                ParentId = itemCreate.ParentId,
                ItemTemplateId = itemCreate.ItemTemplateId,
                SerialNumber = itemCreate.SerialNumber,
                LocationId = itemCreate.LocationId,
                VendorId = itemCreate.VendorId,
                CreatedById = itemCreate.CreatedById,
                Comment = itemCreate.Comment,
                CreatedDate = DateTime.Now
            }))
            {
                await context.Items.AddAsync(item);
                if (item.Id != null)
                {
                    createdItemsIds.Add(item.Id);
                    if (item.CreatedById != null)
                        await CreateLogEntryAsync(item.Id, item.CreatedById, "Item added");
                }

                await context.SaveChangesAsync();
            }
            return createdItemsIds;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task UpdateItemAsync(string updatedById, Item updatedItem)
    {
        try
        {
            var item = await context.Items
                .Include(item => item.Location)
                .Include(item => item.Vendor)
                .FirstOrDefaultAsync(c => c.Id == updatedItem.Id);

            if (item?.Id != null)
            {
                if (updatedItem.WpId != item.WpId)
                {
                    await CreateLogEntryAsync(item.Id, updatedById, $"WellPartner ID changed from {item.WpId} to {updatedItem.WpId}");
                    item.WpId = updatedItem.WpId;
                }

                if (updatedItem.SerialNumber != item.SerialNumber)
                {
                    await CreateLogEntryAsync(item.Id, updatedById, $"Serial number changed from {item.SerialNumber} to {updatedItem.SerialNumber}");
                    item.SerialNumber = updatedItem.SerialNumber;
                }

                if (updatedItem.LocationId != item.LocationId && updatedItem.LocationId != null)
                {
                    var location = await context.Locations.FirstOrDefaultAsync(c => c.Id == updatedItem.LocationId);
                    await CreateLogEntryAsync(item.Id, updatedById, $"Location changed from {item.Location?.Name} to {location?.Name}");
                    item.LocationId = updatedItem.LocationId;
                }

                if (updatedItem.ParentId != item.ParentId)
                {
                    var oldParent = await context.Items.FirstOrDefaultAsync(c => c.Id == item.ParentId);
                    var newParent = await context.Items.FirstOrDefaultAsync(c => c.Id == updatedItem.ParentId);

                    item.ParentId = updatedItem.ParentId;
                    await CreateLogEntryAsync(item.Id, updatedById, $"Parent ID changed from {oldParent?.WpId} to {newParent?.WpId}");

                    if (oldParent?.Id != null)
                        await CreateLogEntryAsync(oldParent.Id, updatedById,
                            $"Item {updatedItem.Id} removed from parent");

                    if (newParent?.Id != null)
                        await CreateLogEntryAsync(newParent.Id, updatedById,
                            $"Item {updatedItem.Id} added to parent");
                }

                if (updatedItem.VendorId != item.VendorId && updatedItem.VendorId != null)
                {
                    var vendor = await context.Vendors.FirstOrDefaultAsync(c => c.Id == updatedItem.VendorId);
                    await CreateLogEntryAsync(item.Id, updatedById, $"Vendor changed from {item.Vendor?.Name} to {vendor?.Name}");
                    item.VendorId = updatedItem.VendorId;
                }

                item.CreatedById = updatedItem.CreatedById;
                item.Comment = updatedItem.Comment;
                item.UpdatedDate = DateTime.Now;

                await context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task AddChildItemToParentAsync(string parentItemId, string childItemId)
    {
        try
        {
            var parentItem = await context.Items.Include(item => item.Children).FirstOrDefaultAsync(i => i.Id == parentItemId);
            var childItem = await context.Items.FirstOrDefaultAsync(i => i.Id == childItemId);

            if (parentItem != null && childItem != null)
            {
                parentItem.Children ??= new Collection<Item>();
                parentItem.Children.Add(childItem);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task RemoveParentIdAsync(string itemId)
    {
        try
        {
            var item = await context.Items.FirstOrDefaultAsync(i => i.Id == itemId);

            if (item != null)
            {
                item.ParentId = null;
                await context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<string>> DeleteItemAsync(string id, bool? deleteSubItems)
    {
        try
        {
            var itemIds = new List<string>();
            var item = await context.Items.Include(c => c.Children).FirstOrDefaultAsync(c => c.Id == id);
            if (item != null)
            {
                if (item.Children != null)
                    foreach (var child in item.Children)
                    {
                        child.ParentId = null;
                        if (deleteSubItems != true) continue;
                        if (child.Id != null) await DeleteItemAsync(child.Id, deleteSubItems);
                    }

                if (item.Id != null) itemIds.Add(item.Id);
                context.Items.Remove(item);
                await context.SaveChangesAsync();
            }

            return itemIds;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> IsWpIdUnique(string id)
    {
        try
        {
            var item = await context.Items.FirstOrDefaultAsync(c => c.WpId == id);
            return item == null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> IsSerialNumberUnique(string serialNumber)
    {
        try
        {
            var item = await context.Items.FirstOrDefaultAsync(c => c.SerialNumber == serialNumber);
            return item == null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task CreateLogEntryAsync(string itemId, string createdById, string message)
    {
        try
        {
            var logEntry = new LogEntry
            {
                ItemId = itemId,
                CreatedById = createdById,
                Message = message,
                CreatedDate = DateTime.Now
            };

            await context.LogEntries.AddAsync(logEntry);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task ItemsCreated(ICollection<Item> itemsCreated) 
    {
        var sbClient = new ServiceBusClient(generalUtilities.GetSecretValueFromKeyVault("inventory-send-sas"));
        var sender = sbClient.CreateSender(AppSettings.TopicItemEvent);
        using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

        var itemsCreatedContracts = itemsCreated
            .Select(ic => new ItemEventContract{ItemId = ic.Id ?? throw new Exception("Empty item id"), ItemTemplateId = ic.ItemTemplateId ?? throw new Exception("Empty item id"), ParentItemId = ic.ParentId, ItemEvent = checklist_inventory_contracts.Items.Enums.ItemEvent.ItemCreated});

        foreach (var itemCreated in itemsCreatedContracts)
        {
            if (!messageBatch.TryAddMessage(new ServiceBusMessage(JsonSerializer.Serialize(itemCreated))))
            {
                throw new Exception($"The message {itemCreated} is too large to fit in the batch.");
            }
        }

        //var sbMessage = new ServiceBusMessage(ids.ToString());
        await sender.SendMessagesAsync(messageBatch);
    }

    public async Task ItemsDeleted(ICollection<string> itemsDeletedIds)
    {
        var sbClient = new ServiceBusClient(generalUtilities.GetSecretValueFromKeyVault("inventory-send-sas"));

        var sender = sbClient.CreateSender(AppSettings.TopicItemEvent);
        using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

        var itemsDeletedContracts = itemsDeletedIds
            .Select(ic => new ItemEventContract{ItemId = ic ?? throw new Exception("Empty item id"), ItemEvent = checklist_inventory_contracts.Items.Enums.ItemEvent.ItemDeleted});

        foreach (var itemDeleted in itemsDeletedContracts)
        {
            if (!messageBatch.TryAddMessage(new ServiceBusMessage(JsonSerializer.Serialize(itemDeleted))))
            {
                throw new Exception($"The message {itemDeleted} is too large to fit in the batch.");
            }
        }

        //var sbMessage = new ServiceBusMessage(ids.ToString());
        await sender.SendMessagesAsync(messageBatch);
    }
}