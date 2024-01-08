using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.ItemValidations;

public interface IItemCreateValidator
{
    Task<ValidationResult> ValidateAsync(Item item);
}