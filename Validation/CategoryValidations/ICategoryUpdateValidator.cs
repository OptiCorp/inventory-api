using FluentValidation;
using Inventory.Models;

namespace Inventory.Validations.CategoryValidations;

public interface ICategoryUpdateValidator : IValidator<Category>;