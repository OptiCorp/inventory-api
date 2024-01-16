using FluentValidation;
using Inventory.Models;

namespace Inventory.Validations.ItemTemplateValidations;

public interface IItemTemplateCreateValidator : IValidator<ItemTemplate>
{
}