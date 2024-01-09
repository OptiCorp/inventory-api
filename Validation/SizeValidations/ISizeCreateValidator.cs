using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.SizeValidations;

public interface ISizeCreateValidator
{
    Task<ValidationResult> ValidateAsync(Size size);
}