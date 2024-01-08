using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.ItemTemplateValidations;

public interface IItemTemplateUpdateValidator
{
    Task<ValidationResult> ValidateAsync(ItemTemplate itemTemplate);
}