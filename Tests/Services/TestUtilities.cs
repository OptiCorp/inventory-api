using Microsoft.EntityFrameworkCore;
using Inventory.Models;

namespace Inventory.Tests.Services
{
    public class TestUtilities
    {
        public async Task<InventoryDbContext> GetDbContext(string testType)
        {
            var options = new DbContextOptionsBuilder<InventoryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new InventoryDbContext(options);
            databaseContext.Database.EnsureCreated();

            if (testType == "User")
            {
                if (await databaseContext.User.CountAsync() <= 0)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        await databaseContext.User.AddAsync(
                            new User
                            {
                                Id = string.Format("User {0}", i),
                                AzureAdUserId = string.Format("AzureAD{0}@bouvet.no", i),
                                UserRole = i % 2 == 0 ? "Inspector" : "Leader",
                                FirstName = "name",
                                LastName = "nameson",
                                Email = "some email",
                                Username = string.Format("Username {0}", i),
                                Status = i % 5 == 0 ? UserStatus.Deleted : UserStatus.Active,
                                CreatedDate = TimeZoneInfo.ConvertTime(DateTime.Now,
                                    TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"))
                            }
                        );
                    }

                    await databaseContext.SaveChangesAsync();
                }

                return databaseContext;
            }

            await databaseContext.User.AddRangeAsync(
                new User
                {
                    Id = "User 1",
                    AzureAdUserId = "Some email",
                    UserRole = "Inspector",
                    FirstName = "name",
                    LastName = "nameson",
                    Email = "some email",
                    Username = "username1",
                    Status = UserStatus.Active,
                    CreatedDate = TimeZoneInfo.ConvertTime(DateTime.Now,
                        TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"))
                },
                new User
                {
                    Id = "User 2",
                    AzureAdUserId = "Some email",
                    UserRole = "Leader",
                    FirstName = "name",
                    LastName = "nameson",
                    Email = "some email",
                    Username = "username2",
                    Status = UserStatus.Active,
                    CreatedDate = TimeZoneInfo.ConvertTime(DateTime.Now,
                        TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"))
                },
                new User
                {
                    Id = "User 3",
                    AzureAdUserId = "Some email",
                    UserRole = "Inspector",
                    FirstName = "name",
                    LastName = "nameson",
                    Email = "some email",
                    Username = "username3",
                    Status = UserStatus.Active,
                    CreatedDate = TimeZoneInfo.ConvertTime(DateTime.Now,
                        TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"))
                }
            );

            if (testType == "Item")
            {
                if (await databaseContext.Items.CountAsync() <= 0)
                {
                    var itemsToAdd = new List<Item>();
                    for (int i = 0; i < 10; i++)
                    {
                        var newItem = new Item
                        {
                            Id = string.Format("Item {0}", i),
                            WpId = string.Format("ItemWpId {0}", i),
                            ParentId = string.Format("ItemParentId {0}", i),
                            UserId = string.Format("User {0}", i),
                            DocumentationId = string.Format("ItemDocumentationId {0}", i),
                            Description = string.Format("ItemDescription {0}", i),
                            Comment = string.Format("ItemComment {0}", i),
                            // LocationId = string.Format("ItemLocation {0}", i),
                            SerialNumber = string.Format("ItemSerialNumber {0}", i),
                            ProductNumber = string.Format("ItemProductNumber {0}", i),
                            // VendorId = string.Format("ItemVendor {0}", i),
                            Type = string.Format("ItemType {0}", i),
                            Children = new List<Item>(),
                            CreatedDate = TimeZoneInfo.ConvertTime(DateTime.Now,
                                TimeZoneInfo.FindSystemTimeZoneById(("Central European Standard Time"))),
                        };
 
                        for (int j = 0; j < 10; j++)
                        {
                            var childItem = new Item
                            {
                                Id = $"{i}-{j}",
                                WpId = $"ItemChildWpId {i}-{j}",
                                ParentId = newItem.Id,
                                UserId = $"ItemChildUserId {i}-{j}", 
                                DocumentationId = $"ItemChildDocumentationId {i}-{j}",
                                Description = $"ItemChildDescription {i}-{j}",
                                Comment = $"ItemChildComment {i}-{j}",
                                LocationId = $"ItemChildLocation {i}-{j}",
                                SerialNumber = $"ItemChildSerialNumber {i}-{j}",
                                ProductNumber = $"ItemChildProductNumber {i}-{j}",
                                VendorId = $"ItemChildVendor {i}-{j}",
                                Type = $"ItemChildType {i}-{j}",
                                Parent = newItem,
                                CreatedDate = TimeZoneInfo.ConvertTime(DateTime.Now,
                                    TimeZoneInfo.FindSystemTimeZoneById(("Central European Standard Time"))),
                                
                            };
                            newItem.Children.ToList().Add(childItem);
                        }

                        itemsToAdd.Add(newItem);
                    }

                    await databaseContext.Items.AddRangeAsync(itemsToAdd);
                }
            }
            await databaseContext.SaveChangesAsync();

            return databaseContext;
        }
    }
}