using Microsoft.AspNetCore.Mvc;
using Inventory.Models;
using Inventory.Services;
using Swashbuckle.AspNetCore.Annotations;
using FluentValidation.Results;
using Inventory.Validations.UserRoleValidations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Inventory.Controllers
{
    [ApiController]
    [Route("api")]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;
        private readonly IUserRoleCreateValidator _createValidator;
        private readonly IUserRoleUpdateValidator _updateValidator;

        public UserRoleController(IUserRoleService userRoleService, IUserRoleCreateValidator createValidator, IUserRoleUpdateValidator updateValidator)
        {
            _userRoleService = userRoleService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all user roles", Description = "Retrieves a list of all user roles.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<UserRole>))]
        public async Task<IActionResult> GetUserRoles()
        {
            return Ok(await _userRoleService.GetAllUserRolesAsync());
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get user role by ID", Description = "Retrieves a user role by the ID.")]
        [SwaggerResponse(200, "Success", typeof(UserRole))]
        [SwaggerResponse(404, "User role not found")]
        public async Task<IActionResult> GetUserRoleById(string id)
        {
            var userRole = await _userRoleService.GetUserRoleByIdAsync(id);

            if (userRole == null)
            {
                return NotFound("User role not found");
            }
            return Ok(userRole);
        }
        
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new user role", Description = "Create a new user role")]
        [SwaggerResponse(201, "User role created", typeof(UserRole))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<IActionResult> CreateUserRole(UserRole userRole)
        {
            ValidationResult validationResult = await _createValidator.ValidateAsync(userRole);

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
            
            var newUserRoleId = await _userRoleService.CreateUserRoleAsync(userRole);
            var newUserRole = await _userRoleService.GetUserRoleByIdAsync(newUserRoleId);
            return CreatedAtAction(nameof(GetUserRoleById), new { id = newUserRoleId }, newUserRole);

        }
        
        [HttpPut]
        [SwaggerOperation(Summary = "Update user role by ID", Description = "Updates an existing user role by its ID.")]
        [SwaggerResponse(200, "User role updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "User role not found")]
        public async Task<IActionResult> UpdateUserRole(UserRole updatedUserRole)
        {
            ValidationResult validationResult = await _updateValidator.ValidateAsync(updatedUserRole);

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

            var userRole = await _userRoleService.GetUserRoleByIdAsync(updatedUserRole.Id);

            if (userRole == null)
            {
                return NotFound("User role not found");
            }

            await _userRoleService.UpdateUserRoleAsync(updatedUserRole);

            return Ok($"User role updated, changed name to '{updatedUserRole.Name}'.");
        }
        
        [HttpDelete]
        [SwaggerOperation(Summary = "Delete user role by ID", Description = "Deletes a user role by their ID.")]
        [SwaggerResponse(200, "User role deleted")]
        [SwaggerResponse(404, "User role not found")]
        public async Task<IActionResult> DeleteUserRole(string id)
        {
            UserRole userRoleToDelete = await _userRoleService.GetUserRoleByIdAsync(id);

            if (userRoleToDelete == null)
            {
                return NotFound("User role not found");
            }

            if (await _userRoleService.IsUserRoleInUseAsync(userRoleToDelete))
            {
                return Conflict($"Conflict: Unable to delete the {userRoleToDelete.Name} role.\nReason: There are users currently assigned to this role.");
            }
            
            await _userRoleService.DeleteUserRoleAsync(userRoleToDelete.Id);

            return Ok($"User role: '{userRoleToDelete.Name}' deleted.");
        }
    }

}
