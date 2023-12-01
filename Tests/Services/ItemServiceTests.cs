using Inventory.Models;
using Inventory.Models.DTOs.ItemDtos;
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
            TestUtilities testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Item");
            ItemUtilities itemUtilities = new ItemUtilities();
            ItemService itemService = new ItemService(dbContext, itemUtilities);
            
            // Act
            var items = await itemService.GetAllItemsAsync();
            
            // Assert
            Assert.IsType<List<ItemResponseDto>>(items);
            Assert.Equal(10, items.Count());
        }
        // TODO: Add when there are user ids in db
        // [Fact]
        // public async void ItemService_GetAllItemsByUserId_ReturnsItemList()
        // {
        //     // Arrange
        //     TestUtilities testUtilities = new TestUtilities();
        //     var dbContext = await testUtilities.GetDbContext("Item");
        //     ItemUtilities itemUtilities = new ItemUtilities();
        //     ItemService itemService = new ItemService(dbContext, itemUtilities);
        //
        //     var userId1 = "User 2";
        //     var userId2 = "User 10";
        //     var page1 = 1;
        //     var page2 = 10;
        //     
        //     // Act
        //     var itemsByUserId1 = await itemService.GetAllItemsByUserIdAsync(userId1, page1);
        //     var itemsByUserId2 = await itemService.GetAllItemsByUserIdAsync(userId2, page2);
        //     
        //     // Assert 
        //     Assert.IsType<List<ItemResponseDto>>(itemsByUserId1);
        //     /*Assert.Equal(itemsByUserId1.Count(), 10);
        //     Assert.Equal(itemsByUserId2.Count(), 10);*/
        // }
        
        [Fact]
        public async Task ItemService_GetAllItemsBySearchString_ReturnsItemList()
        {
            // Arrange
            TestUtilities testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Item");
            ItemUtilities itemUtilities = new ItemUtilities();
            ItemService itemService = new ItemService(dbContext, itemUtilities);
            
            // Act
            var items = await itemService.GetAllItemsBySearchStringAsync("a", 1);
            
            //Assert
            Assert.IsType<List<ItemResponseDto>>(items);
            Assert.Equal(10, items.Count());
        }
        
        

        [Fact]
        public async Task ItemService_GetChildren_ReturnsItemList()
        {
            // Arrange
            TestUtilities testUtilities = new TestUtilities();
            ItemUtilities itemUtilities = new ItemUtilities();
            var dbContext = await testUtilities.GetDbContext("Item");
            var parentId = "parentId 1";
            
            
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
                    Vendor = "TestVendor",
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
            TestUtilities testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Item");
            ItemUtilities itemUtilities = new ItemUtilities();
            ItemService itemService = new ItemService(dbContext, itemUtilities);
            
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
            TestUtilities testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Item");
            ItemUtilities itemUtilities = new ItemUtilities();
            ItemService itemService = new ItemService(dbContext, itemUtilities);
            
            var newTestItem1 = new ItemCreateDto()
            {
                ProductNumber = "123",
                Description = "ADescription",
                SerialNumber = "321",
                WpId = "456",
                Comment = "AComment",
                Location = "ALocation",
                ParentId = "789",
                Type = "AType",
                Vendor = "AVendor",
                AddedById = "654",
            };
            
            var newTestItem2 = new ItemCreateDto()
            {
                ProductNumber = "456",
                Description = "ADescription",
                SerialNumber = "321",
                WpId = "789",
                Comment = "AComment",
                Location = "ALocation",
                ParentId = "789",
                Type = "BType",
                Vendor = "AVendor",
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
            ItemUtilities itemUtilities = new ItemUtilities();
            ItemService itemService = new ItemService(dbContext, itemUtilities);

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
                Location = "ALocation2",
                Type = "AType2",
                Vendor = "AVendor2"
            };
            
            // Act
            await itemService.UpdateItemAsync(updatedItem);
            var item = await itemService.GetItemByIdAsync("Item 1");
            
            // Assert
            Assert.Equal(item.Description, "Item 10");
            Assert.Equal(item.Comment, "Item Comment 1");
        }

        [Fact]
        public async void ItemService_DeleteItem_ReturnsVoid()
        {
            // Arrange
            var testUtilities = new TestUtilities();
            var dbContext = await testUtilities.GetDbContext("Item");
            ItemUtilities itemUtilities = new ItemUtilities();
            ItemService itemService = new ItemService(dbContext, itemUtilities);

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
