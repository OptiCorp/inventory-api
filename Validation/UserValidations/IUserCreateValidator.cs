using FluentValidation;
using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.UserValidations;

public interface IUserCreateValidator : IValidator<User>
{
}