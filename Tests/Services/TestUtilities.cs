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
                                CreatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"))
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
                    CreatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"))
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
                    CreatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"))
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
                    CreatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"))
                }
            );

            await databaseContext.SaveChangesAsync();

            return databaseContext;
        }
    }
}
