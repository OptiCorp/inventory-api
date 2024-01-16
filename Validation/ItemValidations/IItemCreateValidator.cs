using FluentValidation;
using Inventory.Models;

namespace Inventory.Validations.ItemValidations;

public interface IItemCreateValidator : IValidator<Item>
{
}