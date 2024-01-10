using FluentValidation;
using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.UserRoleValidations;

public interface IUserRoleUpdateValidator : IValidator<UserRole>
{
}