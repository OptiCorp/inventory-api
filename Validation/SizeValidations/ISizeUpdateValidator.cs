using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.SizeValidations;

public interface ISizeUpdateValidator
{
    Task<ValidationResult> ValidateAsync(Size size);
}