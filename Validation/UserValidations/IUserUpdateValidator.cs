using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.UserValidations;

public interface IUserUpdateValidator
{
    Task<ValidationResult> ValidateAsync(User user);
}