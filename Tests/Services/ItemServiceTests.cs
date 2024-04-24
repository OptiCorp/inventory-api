using Inventory.Models;
using Inventory.Models.DTO;
using Inventory.Services;
using Xunit;
using Inventory.Utilities;
using Inventory.Common.Models;

namespace Inventory.Tests.Services;

public class ItemServiceTests
{
    [Fact]
    public async void ItemService_GetAllItems_ReturnsItemList()
    {
        // Arrange
        var dbContext = await TestUtilities.GetDbContext("Item");
        var generalUtilities = new GeneralUtilities();
        var itemService = new ItemService(dbContext, generalUtilities);

        // Act
        var items = await itemService.GetAllItemsAsync();

        // Assert
        Assert.IsType<List<Item>>(items);
        Assert.Equal(10, items.Count());
    }

    [Fact]
    public async void ItemService_GetAllItemsByUserId_ReturnsItemList()
    {
        // Arrange
        var dbContext = await TestUtilities.GetDbContext("Item");
        var generalUtilities = new GeneralUtilities();
        var itemService = new ItemService(dbContext, generalUtilities);

        const string userId1 = "User 2";
        const string userId2 = "User 15";
        const int page1 = 1;
        const int page2 = 1;

        // Act
        var itemsByUserId1 = await itemService.GetAllItemsByUserIdAsync(userId1, page1);
        var itemsByUserId2 = await itemService.GetAllItemsByUserIdAsync(userId2, page2);

        // Assert 
        Assert.IsType<List<Item>>(itemsByUserId1);
        Assert.Single(itemsByUserId1);
        Assert.Empty(itemsByUserId2);
    }

    [Fact]
    public async Task ItemService_GetAllItemsBySearchString_ReturnsItemList()
    {
        // Arrange
        var dbContext = await TestUtilities.GetDbContext("Item");
        var generalUtilities = new GeneralUtilities();
        var itemService = new ItemService(dbContext, generalUtilities);

        // Act
        var paginatedItems = await itemService.GetAllItemsBySearchStringAsync("a", 1, 10);  

        //Assert
        Assert.IsType<PaginatedList<Item>>(paginatedItems);
        Assert.Equal(10, paginatedItems.Items.Count());
    }



    [Fact]
    public async Task ItemService_GetChildren_ReturnsItemList()
    {
        // Arrange
        var dbContext = await TestUtilities.GetDbContext("Item");
        const string parentId = "parentId 1";


        var expectedItems = new List<Item>
        {
            new()
            {
                ParentId = parentId,
                Children = new List<Item>(),
                CreatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time")),
                SerialNumber = "TestSerialNumber",
                VendorId = "TestVendor",
                WpId = "TestWpId"

            }
        };

        dbContext.Items.AddRange(expectedItems);
        await dbContext.SaveChangesAsync();

        // Act
        var generalUtilities = new GeneralUtilities();
        var itemService = new ItemService(dbContext, generalUtilities);
        var result = await itemService.GetChildrenAsync(parentId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<Item>>(result);

        var enumerable = result as Item[] ?? result.ToArray();
        Assert.Equal(expectedItems.Count, enumerable.Length);

        var firstResultItem = enumerable.FirstOrDefault();
        Assert.NotNull(firstResultItem);
    }



    [Fact]
    public async void ItemService_GetItemById_ReturnsItem()
    {
        //Arrange
        var dbContext = await TestUtilities.GetDbContext("Item");
        var generalUtilities = new GeneralUtilities();
        var itemService = new ItemService(dbContext, generalUtilities);

        //Act
        var item = await itemService.GetItemByIdAsync("Item 1");

        //Assert
        Assert.IsType<Item>(item);
        Assert.Equal("Item 1", item.Id);
    }

    [Fact]
    public async void ItemService_CreateItem_ReturnsString()
    {
        //Arrange
        var dbContext = await TestUtilities.GetDbContext("Item");
        var generalUtilities = new GeneralUtilities();
        var itemService = new ItemService(dbContext, generalUtilities);

        var newTestItem1 = new ItemCreateDto
        {
            SerialNumber = "321",
            WpId = "456",
            Comment = "AComment",
            LocationId = "ALocation",
            ParentId = "789",
            VendorId = "AVendor",
            CreatedById = "654"
        };

        var newTestItem2 = new ItemCreateDto
        {
            SerialNumber = "321",
            WpId = "789",
            Comment = "AComment",
            LocationId = "ALocation",
            ParentId = "789",
            VendorId = "AVendor",
            CreatedById = "654"
        };
        var itemsToCreate = new List<ItemCreateDto> { newTestItem1, newTestItem2 };

        //Act
        var newItemIds = await itemService.CreateItemAsync(itemsToCreate);
        var items = await itemService.GetAllItemsAsync();

        //Assert
        Assert.IsType<List<string>>(newItemIds);
        Assert.Equal(12, items.Count());
        Assert.Equal(2, newItemIds.Count);
    }

    [Fact]
    public async void ItemService_UpdateItem_ReturnsVoid()
    {
        // Arrange
        var dbContext = await TestUtilities.GetDbContext("Item");
        var generalUtilities = new GeneralUtilities();
        var itemService = new ItemService(dbContext, generalUtilities);

        var updatedItem = new Item
        {
            Id = "Item 1",
            WpId = "5",
            ParentId = "4",
            CreatedById = "2",
            Comment = "Item Comment 1",
            SerialNumber = "456"
        };

        // Act
        await itemService.UpdateItemAsync(updatedItem.CreatedById, updatedItem);
        var item = await itemService.GetItemByIdAsync("Item 1");

        // Assert
        Assert.Equal("Item Comment 1", item?.Comment);
    }

    [Fact]
    public async void ItemService_DeleteItem_ReturnsVoid()
    {
        // Arrange
        var dbContext = await TestUtilities.GetDbContext("Item");
        var generalUtilities = new GeneralUtilities();
        var itemService = new ItemService(dbContext, generalUtilities);

        const string itemId = "Item 1";

        // Act
        await itemService.DeleteItemAsync(itemId, false);
        var allItems = await itemService.GetAllItemsAsync();

        // Assert
        var deletedItem = allItems.FirstOrDefault(i => i.Id == itemId);
        Assert.Null(deletedItem);
    }
}