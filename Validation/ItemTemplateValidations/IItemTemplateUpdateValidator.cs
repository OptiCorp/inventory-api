using FluentValidation;
using Inventory.Models;

namespace Inventory.Validations.ItemTemplateValidations;

public interface IItemTemplateUpdateValidator : IValidator<ItemTemplate>;