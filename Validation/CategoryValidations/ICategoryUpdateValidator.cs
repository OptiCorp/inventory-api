using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.CategoryValidations;

public interface ICategoryUpdateValidator
{
    Task<ValidationResult> ValidateAsync(Category category);
}