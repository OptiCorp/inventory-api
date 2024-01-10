using FluentValidation;
using FluentValidation.Results;
using Inventory.Models;
using Inventory.Services;

namespace Inventory.Validations.UserRoleValidations
{
    public class UserRoleCreateValidator : AbstractValidator<UserRole>, IUserRoleCreateValidator
    {
        private readonly IUserRoleService _userRoleService;
        public UserRoleCreateValidator(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
            
            RuleFor(userRole => userRole.Name).NotEmpty().NotNull().WithMessage("User role name cannot be empty.")
                .MinimumLength(3).WithMessage("User role name must be at least 3 characters.")
                .MaximumLength(50).WithMessage("User role name cannot exceed 50 characters.")
                .Matches("^[a-zA-Z_\\- ]+$").WithMessage("User role name can only contain letters, spaces, underscores or hyphens.")
                .MustAsync(async (name, cancellation) => !await _userRoleService.IsUserRoleNameTaken(name)).WithMessage("This user role name is taken");
        }
    }
}