using FluentValidation;
using Inventory.Models;

namespace Inventory.Validations.LocationValidations;

public interface ILocationUpdateValidator : IValidator<Location>
{
}