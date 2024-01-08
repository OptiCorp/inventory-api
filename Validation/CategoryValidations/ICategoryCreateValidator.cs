using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.CategoryValidations;

public interface ICategoryCreateValidator
{
    Task<ValidationResult> ValidateAsync(Category category);
}