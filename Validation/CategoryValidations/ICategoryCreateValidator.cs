using FluentValidation;
using Inventory.Models.DTO;

namespace Inventory.Validations.CategoryValidations;

public interface ICategoryCreateValidator : IValidator<CategoryCreateDto>
{
}