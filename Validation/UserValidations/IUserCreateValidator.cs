using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.UserValidations;

public interface IUserCreateValidator
{
    Task<ValidationResult> ValidateAsync(User user);
}