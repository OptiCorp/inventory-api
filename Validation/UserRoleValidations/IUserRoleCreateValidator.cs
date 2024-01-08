using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.UserRoleValidations;

public interface IUserRoleCreateValidator
{
    Task<ValidationResult> ValidateAsync(UserRole role);
}