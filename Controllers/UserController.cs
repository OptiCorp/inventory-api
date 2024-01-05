using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Azure.Identity;
using Inventory.Models;
using Inventory.Services;
using Inventory.Utilities;
using Microsoft.Graph;

namespace Inventory.Controllers
{
    [ApiController]
    [Route("api")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;
        private readonly IUserUtilities _userUtilities;

        public UserController(IUserService userService, IUserRoleService userRoleService, IUserUtilities userUtilities)
        {
            _userService = userService;
            _userRoleService = userRoleService;
            _userUtilities = userUtilities;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all users", Description = "Retrieves a list of all users.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<User>))]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _userService.GetAllUsersAsync());
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get user by ID", Description = "Retrieves a user by their ID.")]
        [SwaggerResponse(200, "Success", typeof(User))]
        [SwaggerResponse(404, "User not found")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }

        [HttpGet("ByAzureAdUserId/{azureAdUserId}")]
        [SwaggerOperation(Summary = "Get Azure AD user by ID", Description = "Retrieves a Azure AD user by their ID.")]
        [SwaggerResponse(200, "Success", typeof(User))]
        [SwaggerResponse(404, "Azure AD user not found")]
        public async Task<IActionResult> GetUserByAzureAdUserId(string azureAdUserId)
        {
            var user = await _userService.GetUserByAzureAdUserIdAsync(azureAdUserId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }

        [HttpGet("ByUserName/{username}")]
        [SwaggerOperation(Summary = "Get user by username", Description = "Retrieves a user by their username.")]
        [SwaggerResponse(200, "Success", typeof(User))]
        [SwaggerResponse(404, "User not found")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new user", Description = "Creates a new user.")]
        [SwaggerResponse(201, "User created", typeof(User))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<IActionResult> CreateUser(User userCreate, [FromServices] IValidator<UserCreateDto> validator)
        {
            try {
                string[] scopes = new[] { "https://graph.microsoft.com/.default" };
                var graphClient = new GraphServiceClient(new ChainedTokenCredential(
                                    new ManagedIdentityCredential(Environment.GetEnvironmentVariable("AZURE_CLIENT_ID")),
                                    new EnvironmentCredential()),scopes);
                var body = new Microsoft.Graph.Models.Invitation
                {
                    InvitedUserEmailAddress = userCreate.Email,
                    InviteRedirectUrl = "https://um-app-prod.azurewebsites.net/",
                };
                var response = await graphClient.Invitations.PostAsync(body);
            } catch (Exception ex) 
            {
                Console.WriteLine(ex);
                throw;
            }


            if (!string.IsNullOrEmpty(userCreate.UserRoleId))
            {
                ValidationResult validationResult = validator.Validate(userCreate);

                if (!validationResult.IsValid)
                {
                    var modelStateDictionary = new ModelStateDictionary();

                    foreach (ValidationFailure failure in validationResult.Errors)
                    {
                        modelStateDictionary.AddModelError(
                            failure.PropertyName,
                            failure.ErrorMessage
                            );
                    }

                    return ValidationProblem(modelStateDictionary);
                }
            }

            var users = await _userService.GetAllUsersAsync();


            if (_userUtilities.IsUsernameTaken(users, userCreate.Username))
            {
                return Conflict($"The username '{userCreate.Username}' is taken.");
            }

            if (_userUtilities.IsEmailTaken(users, userCreate.Email))
            {
                return Conflict("Invalid email");
            }


            var newUserId = await _userService.CreateUserAsync(userCreate);
            var newUser = await _userService.GetUserByUsernameAsync(userCreate.Username);

            await _userService.UserCreated(newUserId);
            return CreatedAtAction(nameof(GetUserByIdAsync), new { id = newUserId }, newUser);
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Update user by ID", Description = "Updates an existing user by their ID.")]
        [SwaggerResponse(200, "User updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "User not found")]
        public async Task<IActionResult> UpdateUser(User userUpdate, [FromServices] IValidator<UserUpdateDto> validator)
        {
            var users = await _userService.GetAllUsersAsync();
            users = users.Where(u => u.Id != userUpdate.Id);

            ValidationResult validationResult = validator.Validate(userUpdate);

            if (!validationResult.IsValid)
            {
                var modelStateDictionary = new ModelStateDictionary();

                foreach (ValidationFailure failure in validationResult.Errors)
                {
                    modelStateDictionary.AddModelError(
                        failure.PropertyName,
                        failure.ErrorMessage
                        );
                }
                return ValidationProblem(modelStateDictionary);
            }

            if (_userUtilities.IsUsernameTaken(users, userUpdate.Username))
            {
                return BadRequest($"The username '{userUpdate.Username}' is taken.");
            }

            if (_userUtilities.IsEmailTaken(users, userUpdate.Email))
            {
                return BadRequest("Invalid email.");
            }

            await _userService.UpdateUserAsync(userUpdate);

            await _userService.UserUpdated(userUpdate.Id);

            return Ok("User updated");
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Soft delete user by ID", Description = "Deletes a user by their ID, sets the status of the user as \"deleted\" in the system without actually removing them from the database.")]
        [SwaggerResponse(200, "User deleted")]
        [SwaggerResponse(404, "User not found")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user.Status.ToString() == "Deleted")
            {
                return BadRequest("User already deleted");
            }
            if (user == null)
            {
                return NotFound("User not found");
            }

            await _userService.DeleteUserAsync(id);
            await _userService.UserDeleted(id, DeleteMode.Soft);

            return Ok($"User: '{user.Username}' deleted");
        }

        [HttpDelete("HardDeleteUser/{id}")]
        [SwaggerOperation(Summary = "Hard delete user by ID", Description = "Deletes a user by their ID, permanently deletes the user from the system, including removing their data from the database")]
        [SwaggerResponse(200, "User deleted")]
        [SwaggerResponse(404, "User not found")]
        public async Task<IActionResult> HardDeleteUser(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            await _userService.HardDeleteUserAsync(id);
            await _userService.UserDeleted(id, DeleteMode.Hard);

            return Ok($"User: '{user.Username}' deleted");
        }
    }
}
