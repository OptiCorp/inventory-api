using FluentValidation;
using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.ItemTemplateValidations;

public interface IItemTemplateUpdateValidator : IValidator<ItemTemplate>
{
}