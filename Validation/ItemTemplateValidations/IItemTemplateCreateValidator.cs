using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.ItemTemplateValidations;

public interface IItemTemplateCreateValidator
{
    Task<ValidationResult> ValidateAsync(ItemTemplate itemTemplate);
}