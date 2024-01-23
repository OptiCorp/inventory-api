using FluentValidation;
using Inventory.Models.DTO;

namespace Inventory.Validations.ItemTemplateValidations;

public interface IItemTemplateCreateValidator : IValidator<ItemTemplateCreateDto>
{
}