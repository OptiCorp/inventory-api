using FluentValidation;
using Inventory.Models;

namespace Inventory.Validations.ItemValidations;

public interface IItemUpdateValidator : IValidator<Item>;