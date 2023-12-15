using Inventory.Models;
using Inventory.Models.DTOs.ItemDTOs;
using Inventory.Services;
using Inventory.Utilities;
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
            var userUtilities = new UserUtilities();
            var itemUtilities = new ItemUtilities(userUtilities);
            var itemService = new ItemService(dbContext, itemUtilities);
            
            // Act
            var items = await itemService.GetAllItemsAsync();
            
            // Assert
            Assert.IsType<List<ItemResponseDto>>(items);
            Assert.Equal(10, items.Count());
        }
        
        [Fact]
        public async void ItemService_GetAllItemsByUserId_ReturnsItemList()
        {
            // Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Item");
            var userUtilities = new UserUtilities();
            var itemUtilities = new ItemUtilities(userUtilities);
            var itemService = new ItemService(dbContext, itemUtilities);
        
            const string userId1 = "User 2";
            const string userId2 = "User 15";
            const int page1 = 1;
            const int page2 = 1;
            
            // Act
            var itemsByUserId1 = await itemService.GetAllItemsByUserIdAsync(userId1, page1);
            var itemsByUserId2 = await itemService.GetAllItemsByUserIdAsync(userId2, page2);
            
            // Assert 
            Assert.IsType<List<ItemResponseDto>>(itemsByUserId1);
            Assert.Equal(1, itemsByUserId1.Count());
            Assert.Equal(0, itemsByUserId2.Count());
        }
        
        [Fact]
        public async Task ItemService_GetAllItemsBySearchString_ReturnsItemList()
        {
            // Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Item");
            var userUtilities = new UserUtilities();
            var itemUtilities = new ItemUtilities(userUtilities);
            var itemService = new ItemService(dbContext, itemUtilities);
            
            // Act
            var items = await itemService.GetAllItemsBySearchStringAsync("a", 1, null);
            
            //Assert
            Assert.IsType<List<ItemResponseDto>>(items);
            Assert.Equal(10, items.Count());
        }
        
        

        [Fact]
        public async Task ItemService_GetChildren_ReturnsItemList()
        {
            // Arrange
            var testUtilities = new TestUtilities();
            var userUtilities = new UserUtilities();
            var itemUtilities = new ItemUtilities(userUtilities);
            var dbContext = await testUtilities.GetDbContext("Item");
            const string parentId = "parentId 1";
            
            
            var expectedItems = new List<Item>
            {
                new Item
                {
                    ParentId = parentId,
                    Children = new List<Item>(),
                    CreatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time")),
                    Description = "Test Description",
                    ProductNumber = "TestProductNumber",
                    SerialNumber = "TestSerialNumber",
                    VendorId = "TestVendor",
                    WpId = "TestWpId",
                    
                }
            };
            
            dbContext.Items.AddRange(expectedItems);
            await dbContext.SaveChangesAsync();
            
            // Act
            var itemService = new ItemService(dbContext, itemUtilities);
            var result = await itemService.GetChildrenAsync(parentId);
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<ItemResponseDto>>(result);
            
            Assert.Equal(expectedItems.Count, result.Count());

            var firstResultItem = result.FirstOrDefault();
            Assert.NotNull(firstResultItem);
            Assert.Equal(expectedItems[0].Description, firstResultItem.Description);
            Assert.Equal(expectedItems[0].ProductNumber, firstResultItem.ProductNumber);
        }
                
        

        [Fact]
        public async void ItemService_GetItemById_ReturnsItem()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Item");
            var userUtilities = new UserUtilities();
            var itemUtilities = new ItemUtilities(userUtilities);
            var itemService = new ItemService(dbContext, itemUtilities);
            
            //Act
            var item = await itemService.GetItemByIdAsync("Item 1");
            
            //Assert
            Assert.IsType<ItemResponseDto>(item);
            Assert.Equal("ItemProductNumber 1", item.ProductNumber);
            Assert.Equal("Item 1", item.Id);
        }
        
        [Fact]
        public async void ItemService_CreateItem_ReturnsString()
        {
            //Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Item");
            var userUtilities = new UserUtilities();
            var itemUtilities = new ItemUtilities(userUtilities);
            var itemService = new ItemService(dbContext, itemUtilities);
            
            var newTestItem1 = new ItemCreateDto()
            {
                ProductNumber = "123",
                Description = "ADescription",
                SerialNumber = "321",
                WpId = "456",
                Comment = "AComment",
                LocationId = "ALocation",
                ParentId = "789",
                Type = "AType",
                VendorId = "AVendor",
                AddedById = "654",
            };
            
            var newTestItem2 = new ItemCreateDto()
            {
                ProductNumber = "456",
                Description = "ADescription",
                SerialNumber = "321",
                WpId = "789",
                Comment = "AComment",
                LocationId = "ALocation",
                ParentId = "789",
                Type = "BType",
                VendorId = "AVendor",
                AddedById = "654",
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
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Item");
            var userUtilities = new UserUtilities();
            var itemUtilities = new ItemUtilities(userUtilities);
            var itemService = new ItemService(dbContext, itemUtilities);

            var updatedItem = new ItemUpdateDto()
            {
                Id = "Item 1",
                WpId = "5",
                ParentId = "4",
                AddedById = "2",
                Description = "Item 10",
                Comment = "Item Comment 1",
                ProductNumber = "123",
                SerialNumber = "456",
                // LocationId = "ALocation2",
                Type = "AType2",
                // VendorId = "AVendor2"
            };
            
            // Act
            await itemService.UpdateItemAsync(updatedItem.AddedById, updatedItem);
            var item = await itemService.GetItemByIdAsync("Item 1");
            
            // Assert
            Assert.Equal("Item 10", item.Description);
            Assert.Equal("Item Comment 1", item.Comment);
        }

        [Fact]
        public async void ItemService_DeleteItem_ReturnsVoid()
        {
            // Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Item");
            var userUtilities = new UserUtilities();
            var itemUtilities = new ItemUtilities(userUtilities);
            var itemService = new ItemService(dbContext, itemUtilities);

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
