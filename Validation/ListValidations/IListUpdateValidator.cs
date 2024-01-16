using FluentValidation;
using Inventory.Models;

namespace Inventory.Validations.ListValidations;

public interface IListUpdateValidator : IValidator<List>
{
}