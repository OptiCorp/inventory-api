using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.ListValidations;

public interface IListUpdateValidator
{
    Task<ValidationResult> ValidateAsync(List list);
}