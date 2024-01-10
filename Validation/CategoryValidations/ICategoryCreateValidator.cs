using FluentValidation;
using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.CategoryValidations;

public interface ICategoryCreateValidator : IValidator<Category>
{
}