using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Inventory.Models;
using Inventory.Services;

namespace Inventory.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "Get all users", Description = "Retrieves a list of all users.")]
    [SwaggerResponse(200, "Success", typeof(IEnumerable<User>))]
    [SwaggerResponse(400, "Invalid request")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            return Ok(await userService.GetAllUsersAsync());
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get user by ID", Description = "Retrieves a user by their ID.")]
    [SwaggerResponse(200, "Success", typeof(User))]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(404, "User not found")]
    public async Task<IActionResult> GetUserById(string id)
    {
        try
        {
            var user = await userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpGet("ByAzureAdUserId/{azureAdUserId}")]
    [SwaggerOperation(Summary = "Get Azure AD user by ID", Description = "Retrieves a Azure AD user by their ID.")]
    [SwaggerResponse(200, "Success", typeof(User))]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(404, "Azure AD user not found")]
    public async Task<IActionResult> GetUserByAzureAdUserId(string azureAdUserId)
    {
        try
        {
            var user = await userService.GetUserByAzureAdUserIdAsync(azureAdUserId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }

    [HttpGet("ByUserName/{username}")]
    [SwaggerOperation(Summary = "Get user by username", Description = "Retrieves a user by their username.")]
    [SwaggerResponse(200, "Success", typeof(User))]
    [SwaggerResponse(400, "Invalid request")]
    [SwaggerResponse(404, "User not found")]
    public async Task<IActionResult> GetUserByUsername(string username)
    {
        try
        {
            var user = await userService.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }
        catch (Exception e)
        {
            return BadRequest($"Something went wrong: {e.Message}");
        }
    }
}