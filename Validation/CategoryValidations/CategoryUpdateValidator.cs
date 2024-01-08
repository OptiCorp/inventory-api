using FluentValidation;
using FluentValidation.Results;
using Inventory.Models;

namespace Inventory.Validations.CategoryValidations
{
    public class CategoryUpdateValidator : AbstractValidator<Category>, ICategoryUpdateValidator
    {

        public CategoryUpdateValidator()
        {
            RuleFor(category => category.Name).NotEmpty().WithMessage("Category name is required.")
                .NotNull().WithMessage("Category name cannot be null.")
                .MinimumLength(3).WithMessage("Category name must be at least 3 characters.")
                .MaximumLength(40).WithMessage("Category name cannot exceed 40 characters.")
                .Matches("^[a-zA-Z0-9_,.:\\- ]+$").WithMessage("Category name can only contain letters, numbers, underscores, commas, colons, periods or hyphens.");
            
            RuleFor(category => category.Id).NotEmpty().WithMessage("Category Id is required.")
                .NotNull().WithMessage("Category Id cannot be null.");
        }
        
        public async Task<ValidationResult> ValidateAsync(Category category)
        {
            var result = await ValidateAsync(category);
            return result;
        }
    }
}