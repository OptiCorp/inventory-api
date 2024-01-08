using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.ItemValidations;

public interface IItemUpdateValidator
{
    Task<ValidationResult> ValidateAsync(Item item);
}