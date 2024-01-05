using Inventory.Models;
using Inventory.Services;
using Xunit;

namespace Inventory.Tests.Services
{
    public class ItemServiceTests
    {
        [Fact]
        public async void ItemService_GetAllItems_ReturnsItemList()
        {
            // Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Item");
            var itemService = new ItemService(dbContext);
            
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
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Item");
            var itemService = new ItemService(dbContext);
        
            const string userId1 = "User 2";
            const string userId2 = "User 15";
            const int page1 = 1;
            const int page2 = 1;
            
            // Act
            var itemsByUserId1 = await itemService.GetAllItemsByUserIdAsync(userId1, page1);
            var itemsByUserId2 = await itemService.GetAllItemsByUserIdAsync(userId2, page2);
            
            // Assert 
            Assert.IsType<List<Item>>(itemsByUserId1);
            Assert.Equal(1, itemsByUserId1.Count());
            Assert.Equal(0, itemsByUserId2.Count());
        }
        
        [Fact]
        public async Task ItemService_GetAllItemsBySearchString_ReturnsItemList()
        {
            // Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Item");
            var itemService = new ItemService(dbContext);
            
            // Act
            var items = await itemService.GetAllItemsBySearchStringAsync("a", 1, null);
            
            //Assert
            Assert.IsType<List<Item>>(items);
            Assert.Equal(10, items.Count());
        }
        
        

        [Fact]
        public async Task ItemService_GetChildren_ReturnsItemList()
        {
            // Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Item");
            const string parentId = "parentId 1";
            
            
            var expectedItems = new List<Item>
            {
                new Item
                {
                    ParentId = parentId,
                    Children = new List<Item>(),
                    CreatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time")),
                    SerialNumber = "TestSerialNumber",
                    VendorId = "TestVendor",
                    WpId = "TestWpId",
                    
                }
            };
            
            dbContext.Items.AddRange(expectedItems);
            await dbContext.SaveChangesAsync();
            
            // Act
            var itemService = new ItemService(dbContext);
            var result = await itemService.GetChildrenAsync(parentId);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Item>>(result);
            
            Assert.Equal(expectedItems.Count, result.Count());

            var firstResultItem = result.FirstOrDefault();
            Assert.NotNull(firstResultItem);
        }
                
        

        [Fact]
        public async void ItemService_GetItemById_ReturnsItem()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Item");
            var itemService = new ItemService(dbContext);
            
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
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Item");
            var itemService = new ItemService(dbContext);
            
            var newTestItem1 = new Item()
            {
                SerialNumber = "321",
                WpId = "456",
                Comment = "AComment",
                LocationId = "ALocation",
                ParentId = "789",
                VendorId = "AVendor",
                CreatedById = "654",
            };
            
            var newTestItem2 = new Item()
            {
                SerialNumber = "321",
                WpId = "789",
                Comment = "AComment",
                LocationId = "ALocation",
                ParentId = "789",
                VendorId = "AVendor",
                CreatedById = "654",
            };
            var itemsToCreate = new List<Item> { newTestItem1, newTestItem2 };
            
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
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Item");
            var itemService = new ItemService(dbContext);

            var updatedItem = new Item()
            {
                Id = "Item 1",
                WpId = "5",
                ParentId = "4",
                CreatedById = "2",
                Comment = "Item Comment 1",
                SerialNumber = "456",
            };
            
            // Act
            await itemService.UpdateItemAsync(updatedItem.CreatedById, updatedItem);
            var item = await itemService.GetItemByIdAsync("Item 1");
            
            // Assert
            Assert.Equal("Item Comment 1", item.Comment);
        }

        [Fact]
        public async void ItemService_DeleteItem_ReturnsVoid()
        {
            // Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Item");
            var itemService = new ItemService(dbContext);

            const string itemId = "Item 1";
            
            // Act
            await itemService.DeleteItemAsync(itemId);
            var allItems = await itemService.GetAllItemsAsync();
            
            // Assert
            var deletedItem = allItems.FirstOrDefault(i => i.Id == itemId);
            Assert.Null(deletedItem);
        }
    }
}
