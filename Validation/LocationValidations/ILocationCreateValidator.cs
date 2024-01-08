using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.LocationValidations;

public interface ILocationCreateValidator
{
    Task<ValidationResult> ValidateAsync(Location location);
}