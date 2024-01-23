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
            await databaseContext.Database.EnsureCreatedAsync();

            if (testType == "User")
            {
                if (await databaseContext.User.CountAsync() <= 0)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        await databaseContext.User.AddAsync(
                            new User
                            {
                                Id = $"User {i}",
                                AzureAdUserId = $"AzureAD{i}@bouvet.no",
                                FirstName = "name",
                                LastName = "nameson",
                                Email = "some email",
                                Username = $"Username {i}",
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
                            Id = $"Item {i}",
                            WpId = $"ItemWpId {i}",
                            ParentId = $"ItemParentId {i}",
                            CreatedById = $"User {i}",
                            Comment = $"ItemComment {i}",
                            SerialNumber = $"ItemSerialNumber {i}",
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
                                CreatedById = $"ItemChildUserId {i}-{j}",
                                Comment = $"ItemChildComment {i}-{j}",
                                LocationId = $"ItemChildLocation {i}-{j}",
                                SerialNumber = $"ItemChildSerialNumber {i}-{j}",
                                VendorId = $"ItemChildVendor {i}-{j}",
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