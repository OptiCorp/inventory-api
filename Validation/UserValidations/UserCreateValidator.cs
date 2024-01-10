using Azure.Identity;
using FluentValidation;
using FluentValidation.Results;
using Inventory.Common;
using Inventory.Models;
using Inventory.Services;
using Inventory.Validations.UserValidations;

namespace Inventory.Validation.UserValidations
{
    public class UserCreateValidator : AbstractValidator<User>, IUserCreateValidator
    {
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;
        public UserCreateValidator(IUserService userService, IUserRoleService userRoleService)
        {

            _userService = userService;
            _userRoleService = userRoleService;

            RuleFor(user => user.FirstName).NotNull().NotEmpty().Length(1, 50)
                .Matches("^[a-zA-ZæøåÆØÅ ]+$").WithMessage("First name can only contain letters");
            RuleFor(user => user.LastName).NotNull().NotEmpty().Length(1, 50)
                .Matches("^[a-zA-ZæøåÆØÅ ]+$").WithMessage("Last name can only contain letters.");
            RuleFor(user => user.Username).NotNull().NotEmpty()
                .Length(4, 50).WithMessage("Username must be at least 4 characters long.")
                .Matches("^[a-zA-Z0-9_.-]+$")
                .WithMessage("Username can only contain letters, numbers, underscores, periods or hyphens.")
                .MustAsync(async (username, cancellation) => !await _userService.IsUsernameTaken(username)).WithMessage("This username is taken");
            RuleFor(user => user.Email).NotNull().NotEmpty()
                .Matches("^[a-zA-Z0-9_.-@]+$").WithMessage("Email can only contain letters, numbers, underscores, periods or hyphens.")
                .MustAsync(async (email, cancellation) => !await _userService.IsEmailTaken(email)).WithMessage("This email is taken");
            RuleFor(user => user.UserRoleId).NotNull().NotEmpty()
                .Length(5, 36).WithMessage("Invalid user role id.")
                .MustAsync(async (userRoleId, cancellation) => await _userRoleService.IsValidUserRole(userRoleId)).WithMessage("Entered user role was not found.");
        }
    }
}