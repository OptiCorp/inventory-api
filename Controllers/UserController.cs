using Microsoft.AspNetCore.Mvc;
using Inventory.Models;
using Inventory.Services;
using Swashbuckle.AspNetCore.Annotations;
using Inventory.Models.DTO;

namespace inventory_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all users", Description = "Retrieves a list of all users.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<UserDto>))]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUser()
        {
            return Ok(await _userService.GetAllUsersAsync());
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get user", Description = "Retrieves a user.")]
        [SwaggerResponse(200, "Success", typeof(UserDto))]
        [SwaggerResponse(404, "User not found")]
        public async Task<ActionResult<UserDto>> GetUser(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }
        
        [HttpGet("ByAzureId/{id}")]
        [SwaggerOperation(Summary = "Get user by Azure Id", Description = "Retrieves a user by their Azure Id.")]
        [SwaggerResponse(200, "Success", typeof(UserDto))]
        [SwaggerResponse(404, "User not found")]
        public async Task<ActionResult<UserDto>> GetUserByAzureIdAsync(string id)
        {
            var user = await _userService.GetUserByAzureAdUserIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }
    }
}
