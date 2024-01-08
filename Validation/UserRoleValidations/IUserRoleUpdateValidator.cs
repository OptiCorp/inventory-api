using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.UserRoleValidations;

public interface IUserRoleUpdateValidator
{
    Task<ValidationResult> ValidateAsync(UserRole role);
}