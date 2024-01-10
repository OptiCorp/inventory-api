using FluentValidation;
using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.LocationValidations;

public interface ILocationUpdateValidator : IValidator<Location>
{
}