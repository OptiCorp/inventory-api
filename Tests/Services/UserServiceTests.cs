using Inventory.Models;
using Inventory.Services;
using Xunit;

namespace Inventory.Tests.Services;

public class UserServiceTests
{
    [Fact]
    public async void UserService_GetAllUsers_ReturnsUserList()
    {
        //Arrange
        var testUtilities = new TestUtilities();
        var dbContext = await TestUtilities.GetDbContext("User");
        var userService = new UserService(dbContext);

        //Act
        var users = await userService.GetAllUsersAsync();

        //Assert
        Assert.IsType<List<User>>(users);
        Assert.Equal(8, users.Count());
    }

    [Fact]
    public async void UserService_GetAllUsersAdmin_ReturnsUserList()
    {
        //Arrange
        var testUtilities = new TestUtilities();
        var dbContext = await TestUtilities.GetDbContext("User");
        var userService = new UserService(dbContext);

        //Act
        var users = await userService.GetAllUsersAdminAsync();

        //Assert
        Assert.IsType<List<User>>(users);
        Assert.Equal(10, users.Count());
    }

    [Fact]
    public async void UserService_GetUserById_ReturnsUser()
    {
        //Arrange
        var testUtilities = new TestUtilities();
        var dbContext = await TestUtilities.GetDbContext("User");
        var userService = new UserService(dbContext);

        //Act
        var user = await userService.GetUserByIdAsync("User 1");

        //Assert
        Assert.IsType<User>(user);
        Assert.Equal("Username 1", user.Username);
        Assert.Equal("User 1", user.Id);
    }

    [Fact]
    public async void UserService_GetUserByAzureAdUserId_ReturnsUser()
    {
        //Arrange
        var testUtilities = new TestUtilities();
        var dbContext = await TestUtilities.GetDbContext("User");
        var userService = new UserService(dbContext);

        //Act
        var user = await userService.GetUserByAzureAdUserIdAsync("AzureAD1@bouvet.no");

        //Assert
        Assert.IsType<User>(user);
        Assert.Equal("User 1", user.Id);
    }

    [Fact]
    public async void UserService_GetUserByUsername_ReturnsUser()
    {
        //Arrange
        var testUtilities = new TestUtilities();
        var dbContext = await TestUtilities.GetDbContext("User");
        var userService = new UserService(dbContext);

        //Act
        var user = await userService.GetUserByUsernameAsync("Username 1");

        //Assert
        Assert.IsType<User>(user);
        Assert.Equal("User 1", user.Id);
    }
}