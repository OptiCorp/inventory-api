using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.ListValidations;

public interface IListCreateValidator
{
    Task<ValidationResult> ValidateAsync(List list);
}