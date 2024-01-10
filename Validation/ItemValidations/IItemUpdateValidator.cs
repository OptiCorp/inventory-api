using FluentValidation;
using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.ItemValidations;

public interface IItemUpdateValidator : IValidator<Item>
{
}