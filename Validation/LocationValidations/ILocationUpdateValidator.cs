using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.LocationValidations;

public interface ILocationUpdateValidator
{
    Task<ValidationResult> ValidateAsync(Location location);
}