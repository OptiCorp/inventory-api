using FluentValidation;
using FluentValidation.Results;
using Inventory.Common;
using Inventory.Models;
using Inventory.Services;

namespace Inventory.Validations.UserValidations
{
    public class UserUpdateValidator : AbstractValidator<User>, IUserUpdateValidator
    {
        private readonly IUserRoleService _userRoleService;
        private readonly IUserService _userService;
        public UserUpdateValidator(IUserRoleService userRoleService, IUserService userService)
        {

            _userRoleService = userRoleService;
            _userService = userService;

            RuleFor(user => user.FirstName)
                .Length(1, 50)
                .Matches("^[a-zA-ZæøåÆØÅ ]+$").WithMessage("First name can only contain letters.")
                .Unless(user => string.IsNullOrEmpty(user.FirstName));
            RuleFor(user => user.LastName)
                .Length(1, 50)
                .Matches("^[a-zA-ZæøåÆØÅ ]+$").WithMessage("Last name can only contain letters.")
                .Unless(user => string.IsNullOrEmpty(user.LastName));
            RuleFor(user => user.Username)
                .Length(4, 50)
                .When(user => !string.IsNullOrWhiteSpace(user.Username))
                .WithMessage("Username must be atleast 4 characters long.")
                .Matches("^[a-zA-Z0-9_.-]+$").WithMessage("Username can only contain letters, numbers, underscores, periods or hyphens.")
                .MustAsync(async (userName, cancellation) => !await _userService.IsUsernameTaken(userName))
                .Unless(user => string.IsNullOrEmpty(user.Username));
            RuleFor(user => user.UserRoleId)
                .Length(10, 36).WithMessage("Invalid user role id.")
                .MustAsync(async (userRoleId, cancellation) => await _userRoleService.IsValidUserRole(userRoleId)).WithMessage("User role does not exist")
                .Unless(user => string.IsNullOrEmpty(user.UserRoleId));
            RuleFor(user => user.Email)
                .EmailAddress().WithMessage("Invalid email")
                .Matches("^[a-zA-Z0-9_.-@]+$").WithMessage("Email can only contain letters, numbers, underscores, periods or hyphens.")
                .MustAsync(async (email, cancellation) => !await _userService.IsEmailTaken(email)).WithMessage("This email is taken")
                .Unless(user => string.IsNullOrEmpty(user.Email));
        }
    }
}